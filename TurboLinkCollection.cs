using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;

namespace protoc_gen_turbolink
{
    public class GrpcEnumField
    {
        public string Name { get; set; } //eg. "Male", "Female"
        public int Number { get; set; } //eg. "0", "1"
    }

    public class GrpcEnum
    {
        public bool MissingZeroField;
        public string Name { get; set; } //eg. "EGrpcCommonGender"
        public string DisplayName { get; set; } //eg. "Common.Gender"
        public List<GrpcEnumField> Fields { get; set; }
    }

    public abstract class GrpcMessageField
    {
        public readonly FieldDescriptorProto FieldDesc;

        public GrpcMessageField(FieldDescriptorProto fieldDesc)
        {
            FieldDesc = fieldDesc;
            NeedNativeMake = false;
            if (fieldDesc != null)
            {
                FieldGrpcName = fieldDesc.Name.ToLower();
                if (TurboLinkUtils.CppKeyWords.Contains(FieldGrpcName))
                    //add underline if grpc name same as any cpp keywords
                    FieldGrpcName += "_";
            }

            FieldDefaultValue = string.Empty;
        }

        public abstract string FieldType //eg. "int32", "FString", "EGrpcCommonGender", "TArray<FGrpcUserRegisterRequestAddress>"
        {
            get;
        }

        public virtual string FieldGrpcType => FieldDesc.TypeName.Replace(".", "::"); //eg. "::Common::Gender", "::google::protobuf::Value"

        public virtual string FieldName => TurboLinkUtils.GetMessageFieldName(FieldDesc); //eg. "Age", "MyName", "Gender", "AddressArray"

        public string FieldGrpcName { get; set; } //eg. "age", "my_name", "gender", "address_array"

        public abstract string TypeAsNativeField //eg. "TSharedPtr<FGrpcUserRegisterRequestAddress>", "TArray<TSharedPtr<FGrpcUserRegisterRequestAddress>>"
        {
            get;
        }

        public string FieldDefaultValue { get; set; } //eg. "=0", '=""', "= static_cast<EGrpcCommonGender>(0)", ""
        public bool NeedNativeMake { get; set; }
    }

    public class GrpcMessageField_Single : GrpcMessageField
    {
        public GrpcMessageField_Single(FieldDescriptorProto fieldDesc) : base(fieldDesc)
        {
            FieldDefaultValue = TurboLinkUtils.GetFieldDefaultValue(FieldDesc, FieldDesc.HasDefaultValue ? FieldDesc.DefaultValue : null);
        }

        public override string FieldType => TurboLinkUtils.GetFieldType(FieldDesc);

        public override string TypeAsNativeField => NeedNativeMake ? "TSharedPtr<" + TurboLinkUtils.GetFieldType(FieldDesc) + ">" : FieldType;
    }

    public class GrpcMessageField_Repeated : GrpcMessageField
    {
        public readonly GrpcMessageField ItemField;

        public GrpcMessageField_Repeated(FieldDescriptorProto fieldDesc) : base(fieldDesc)
        {
            ItemField = new GrpcMessageField_Single(fieldDesc);
        }

        public override string FieldType => "TArray<" + ItemField.FieldType + ">";

        public override string TypeAsNativeField => NeedNativeMake ? "TArray<TSharedPtr<" + ItemField.FieldType + ">>" : FieldType;
    }

    public class GrpcMessageField_Map : GrpcMessageField
    {
        public readonly GrpcMessageField KeyField;
        public readonly GrpcMessageField ValueField;

        public GrpcMessageField_Map(FieldDescriptorProto fieldDesc, FieldDescriptorProto keyField, FieldDescriptorProto valueField) : base(fieldDesc)
        {
            KeyField = new GrpcMessageField_Single(keyField);
            ValueField = new GrpcMessageField_Single(valueField);
        }

        public override string FieldType => "TMap<" + KeyField.FieldType + ", " + ValueField.FieldType + ">";

        public override string TypeAsNativeField => NeedNativeMake ? "TMap<" + KeyField.FieldType + ", TSharedPtr<" + ValueField.FieldType + ">>" : FieldType;
    }

    public class GrpcMessageField_Oneof : GrpcMessageField
    {
        public readonly GrpcMessage_Oneof OneofMessage;

        public GrpcMessageField_Oneof(GrpcMessage_Oneof oneofMessage) : base(null)
        {
            OneofMessage = oneofMessage;
        }

        public override string FieldType => OneofMessage.Name;

        public override string FieldGrpcType => string.Empty; //should not be called!

        public override string FieldName => OneofMessage.CamelName;

        public override string TypeAsNativeField => string.Empty; //should not be called!
    }

    public class GrpcMessage
    {
        public readonly DescriptorProto MessageDesc;
        public readonly GrpcServiceFile ServiceFile;
        public string[] ParentMessageNameList;

        public GrpcMessage(DescriptorProto messageDesc, GrpcServiceFile serviceFile)
        {
            MessageDesc = messageDesc;
            ServiceFile = serviceFile;
            Fields = new List<GrpcMessageField>();
            UnrealCommentFields = new List<string>();
            HasNativeMake = false;
        }

        public int Index { get; set; }

        public virtual string Name =>
            "F" +
            //	ServiceFile.CamelPackageName +
            TurboLinkUtils.JoinCamelString(ParentMessageNameList, string.Empty) +
            CamelName; //eg. "FGrpcGreeterHelloResponse",  "FGrpcGoogleProtobufValue"

        public virtual string CamelName => TurboLinkUtils.MakeCamelString(MessageDesc.Name); //eg. "HelloResponse", "Value"

        public virtual string GrpcName =>
            ServiceFile.GrpcPackageName + "::" +
            TurboLinkUtils.JoinString(ParentMessageNameList, "::") +
            MessageDesc.Name; //eg. "Greeter::HelloResponse", "google::protobuf::Value"

        public virtual string DisplayName =>
            ServiceFile.CamelPackageName + "." +
            TurboLinkUtils.JoinCamelString(ParentMessageNameList, ".") +
            CamelName; //eg. "Greeter.HelloResponse", "GoogleProtobuf.Value"

        public List<GrpcMessageField> Fields { get; set; }
        public List<string> UnrealCommentFields { get; set; } // class name -> comment
        public bool HasNativeMake { get; set; }
    }

    public class GrpcMessage_Oneof : GrpcMessage
    {
        public readonly OneofDescriptorProto OneofDesc;
        public readonly GrpcEnum OneofEnum;
        public readonly GrpcMessage ParentMessage;

        public GrpcMessage_Oneof(OneofDescriptorProto oneofDesc, GrpcMessage parentMessage, GrpcEnum oneofEnum) : base(null, parentMessage.ServiceFile)
        {
            OneofDesc = oneofDesc;
            ParentMessage = parentMessage;
            OneofEnum = oneofEnum;
        }

        public override string Name => ParentMessage.Name + CamelName; //eg. "FGrpcGoogleProtobufValueKind"

        public override string CamelName => TurboLinkUtils.MakeCamelString(OneofDesc.Name); //eg. "Kind"

        public override string GrpcName => OneofDesc.Name; //eg. "kind"

        public override string DisplayName => ParentMessage.DisplayName + "." + CamelName; //eg. "GoogleProtobuf.Value.Kind"
    }

    public class GrpcServiceMethod
    {
        public readonly MethodDescriptorProto MethodDesc;

        public GrpcServiceMethod(MethodDescriptorProto methodDesc)
        {
            MethodDesc = methodDesc;
        }

        public string Name => MethodDesc.Name;

        public bool ClientStreaming => MethodDesc.ClientStreaming;

        public bool ServerStreaming => MethodDesc.ServerStreaming;

        public string InputType => TurboLinkUtils.GetMessageName(MethodDesc.InputType); //eg. "FGrpcUserRegisterRequest"

        public string GrpcInputType => MethodDesc.InputType.Replace(".", "::"); //eg. "::User::RegisterRequest"

        public string OutputType => TurboLinkUtils.GetMessageName(MethodDesc.OutputType); //eg. "FGrpcUserRegisterResponse"

        public string GrpcOutputType => MethodDesc.OutputType.Replace(".", "::"); //eg. "::User::RegisterResponse"

        public string ContextSuperClass => TurboLinkUtils.GetContextSuperClass(MethodDesc); //eg. "GrpcContext_Ping_Pong", "GrpcContext_Ping_Stream"
    }

    public class GrpcService
    {
        public readonly ServiceDescriptorProto ServiceDesc;

        public GrpcService(ServiceDescriptorProto serviceDesc)
        {
            ServiceDesc = serviceDesc;
        }

        public string Name => ServiceDesc.Name; //eg. "UserService"

        public List<GrpcServiceMethod> MethodArray { get; set; }
    }

    public class GrpcServiceFile
    {
        //split package name as string array
        public readonly string[] PackageNameAsList;
        public readonly FileDescriptorProto ProtoFileDesc;

        public GrpcServiceFile(FileDescriptorProto protoFileDesc)
        {
            ProtoFileDesc = protoFileDesc;
            PackageNameAsList = PackageName.Split('.').ToArray();
            Message2IndexMap = new Dictionary<string, int>();
        }

        public string FileName => ProtoFileDesc.Name; //eg. "hello.proto", "google/protobuf/struct.proto"

        public string CamelFileName => TurboLinkUtils.GetCamelFileName(FileName); //eg. "Hello", "Struct"

        public string PackageName => ProtoFileDesc.Package; //eg. "Greeter", "google.protobuf"

        public string CamelPackageName => string.Join(string.Empty, TurboLinkUtils.MakeCamelStringArray(PackageNameAsList)); //eg. "Greeter", "GoogleProtobuf"

        public string GrpcPackageName => string.Join("::", PackageNameAsList); //eg. "Greeter", "google::protobuf"

        public string TurboLinkBasicFileName => "S" + CamelPackageName + "/" + CamelFileName; //eg. "SGreeter/Hello", "SGoogleProtobuf/Struct"

        public List<string> DependencyFiles { get; set; }
        public List<GrpcEnum> EnumArray { get; set; }
        public List<GrpcMessage> MessageArray { get; set; }
        public List<GrpcService> ServiceArray { get; set; }
        public Dictionary<string, int> Message2IndexMap { get; set; }

        public int GetTotalPingPongMethodCounts()
        {
            var totalPingPongMethodCounts = 0;
            foreach (var service in ServiceArray)
            foreach (var method in service.MethodArray)
                if (!method.ClientStreaming && !method.ServerStreaming)
                    totalPingPongMethodCounts++;

            return totalPingPongMethodCounts;
        }

        public bool NeedBlueprintFunctionLibrary()
        {
            foreach (var message in MessageArray)
                if (message.HasNativeMake || message is GrpcMessage_Oneof)
                    return true;

            return false;
        }
    }

    public class TurboLinkCollection
    {
        //key=ProtoFileName
        public Dictionary<string, GrpcServiceFile> GrpcServiceFiles = new Dictionary<string, GrpcServiceFile>();
        public string InputFileNames;

        public bool AnalysisServiceFiles(CodeGeneratorRequest request, out string error)
        {
            
            error = null;

            var inputFileNames = new StringBuilder();

            //step 1: gather service information
            foreach (var protoFile in request.ProtoFile)
            {
                GrpcServiceFiles.Add(protoFile.Name, new GrpcServiceFile(protoFile));
                inputFileNames.Insert(0, Path.GetFileNameWithoutExtension(protoFile.Name) + "_");
            }

            InputFileNames = inputFileNames.ToString();

            //step 2: imported proto files
            foreach (var protoFileName in GrpcServiceFiles.Keys.ToList()) AddDependencyFiles(protoFileName);

            //setp 3: enum (include nested enum)
            foreach (var protoFileName in GrpcServiceFiles.Keys.ToList()) AddEnums(protoFileName);

            //step 4: message(include nested message and oneof message)
            foreach (var protoFileName in GrpcServiceFiles.Keys.ToList()) AddMessages(protoFileName);
            //step 5: service
            foreach (var protoFileName in GrpcServiceFiles.Keys.ToList()) AddServices(protoFileName);
            //step 6: scan message field to analyze the interdependencies between messages
            foreach (var protoFileName in GrpcServiceFiles.Keys.ToList()) AnalyzeMessage(protoFileName);

            return true;
        }

        private void AddDependencyFiles(string protoFileName)
        {
            var serviceFile = GrpcServiceFiles[protoFileName];

            serviceFile.DependencyFiles = new List<string>();
            foreach (var dependency in serviceFile.ProtoFileDesc.Dependency)
            {
                //set dependency file as turbolink base name, eg. "SGoogleProtobuf/Struct"
                serviceFile.DependencyFiles.Add(GrpcServiceFiles[dependency].TurboLinkBasicFileName);
            }
                
            GrpcServiceFiles[protoFileName] = serviceFile;
        }

        private void AddEnums(string protoFileName)
        {
            var serviceFile = GrpcServiceFiles[protoFileName];
            serviceFile.EnumArray = new List<GrpcEnum>();

            var parentNameList = new string[] { };

            //iterate enum in protofile
            foreach (var enumDesc in serviceFile.ProtoFileDesc.EnumType) AddEnum(ref serviceFile, parentNameList, enumDesc);

            //iterate nested enum in message
            foreach (var message in serviceFile.ProtoFileDesc.MessageType) AddNestedEnums(ref serviceFile, parentNameList, message);

            GrpcServiceFiles[protoFileName] = serviceFile;
        }

        private void AddNestedEnums(ref GrpcServiceFile serviceFile, string[] parentNameList, DescriptorProto message)
        {
            var currentNameList = new string[parentNameList.Length + 1];
            parentNameList.CopyTo(currentNameList, 0);
            currentNameList[parentNameList.Length] = message.Name;

            foreach (var enumDesc in message.EnumType) AddEnum(ref serviceFile, currentNameList, enumDesc);

            if (message.NestedType.Count > 0)
                foreach (var nestedProtoMessage in message.NestedType)
                {
                    if (nestedProtoMessage.Options != null && nestedProtoMessage.Options.MapEntry) continue;
                    AddNestedEnums(ref serviceFile, currentNameList, nestedProtoMessage);
                }
        }

        private void AddEnum(ref GrpcServiceFile serviceFile, string[] parentNameList, EnumDescriptorProto enumDesc)
        {
            var newEnum = new GrpcEnum();

            newEnum.Name = string.Join(string.Empty,
                "E", TurboLinkUtils.JoinString(parentNameList, string.Empty), enumDesc.Name);

            newEnum.DisplayName = serviceFile.CamelPackageName + "." +
                                  TurboLinkUtils.JoinString(parentNameList, ".") +
                                  enumDesc.Name;

            newEnum.Fields = new List<GrpcEnumField>();
            var missingZeroField = true;
            foreach (var enumValue in enumDesc.Value)
            {
                var newEnumField = new GrpcEnumField();
                newEnumField.Name = enumValue.Name;
                newEnumField.Number = enumValue.Number;
                newEnum.Fields.Add(newEnumField);
                if (enumValue.Number == 0) missingZeroField = false;
            }

            newEnum.MissingZeroField = missingZeroField;
            serviceFile.EnumArray.Add(newEnum);
        }

        private void AddMessages(string protoFileName)
        {
            var serviceFile = GrpcServiceFiles[protoFileName];
            serviceFile.MessageArray = new List<GrpcMessage>();

            var parentNameList = new string[] { };
            foreach (var protoMessage in serviceFile.ProtoFileDesc.MessageType) AddMessage(ref serviceFile, parentNameList, protoMessage);
            GrpcServiceFiles[protoFileName] = serviceFile;
        }

        private void AddMessage(ref GrpcServiceFile serviceFile, string[] parentMessageNameList, DescriptorProto protoMessage)
        {
            var message = new GrpcMessage(protoMessage, serviceFile);
            message.ParentMessageNameList = parentMessageNameList;

            //add nested message 
            if (protoMessage.NestedType.Count > 0)
            {
                var currentMessageNameList = new string[parentMessageNameList.Length + 1];
                parentMessageNameList.CopyTo(currentMessageNameList, 0);
                currentMessageNameList[parentMessageNameList.Length] = protoMessage.Name;

                foreach (var nestedProtoMessage in protoMessage.NestedType)
                {
                    if (nestedProtoMessage.Options != null && nestedProtoMessage.Options.MapEntry) continue;
                    AddMessage(ref serviceFile, currentMessageNameList, nestedProtoMessage);
                }
            }

            //add oneof message
            //key=oneof message index in parent message, value.1=enum index in service, value.2=message index in service
            var oneofMessageMap = new Dictionary<int, Tuple<int, int>>();
            if (protoMessage.OneofDecl.Count > 0)
            {
                for (var i = 0; i < protoMessage.OneofDecl.Count; i++)
                {
                    oneofMessageMap.Add(i, new Tuple<int, int>(serviceFile.EnumArray.Count, serviceFile.MessageArray.Count));

                    var oneofEnum = new GrpcEnum();

                    //add oneof message 
                    var oneofMessage = new GrpcMessage_Oneof(protoMessage.OneofDecl[i], message, oneofEnum);
                    oneofMessage.Index = serviceFile.MessageArray.Count;
                    serviceFile.MessageArray.Add(oneofMessage);

                    //add oneof enum
                    oneofEnum.Name = "E" + oneofMessage.Name.Substring(5);
                    oneofEnum.DisplayName = oneofMessage.DisplayName;
                    oneofEnum.Fields = new List<GrpcEnumField>();
                    serviceFile.EnumArray.Add(oneofEnum);
                }
            }


            //add message field
            foreach (var field in protoMessage.Field)
            {
                GrpcMessageField messageField = null;
                bool isMapField;
                FieldDescriptorProto keyField, valueField;
                (isMapField, keyField, valueField) = TurboLinkUtils.IsMapField(field, protoMessage);
                if (isMapField)
                    messageField = new GrpcMessageField_Map(field, keyField, valueField);
                else if (field.Label == FieldDescriptorProto.Types.Label.Repeated)
                    messageField = new GrpcMessageField_Repeated(field);
                else
                    messageField = new GrpcMessageField_Single(field);

                if (field.HasOneofIndex)
                {
                    //add enum field
                    var oneofEnum = serviceFile.EnumArray[oneofMessageMap[field.OneofIndex].Item1];
                    var oneofEnumField = new GrpcEnumField();
                    oneofEnumField.Name = messageField.FieldName;
                    oneofEnumField.Number = oneofEnum.Fields.Count;
                    oneofEnum.Fields.Add(oneofEnumField);

                    //add field to one of message
                    var oneofMessage = (GrpcMessage_Oneof)serviceFile.MessageArray[oneofMessageMap[field.OneofIndex].Item2];
                    if (oneofMessage.Fields.Count == 0)
                    {
                        //first field of oneof zone, add oneof field to parent message
                        var oneofField = new GrpcMessageField_Oneof(oneofMessage);
                        message.Fields.Add(oneofField);
                    }

                    oneofMessage.Fields.Add(messageField);
                }
                else
                {
                    message.Fields.Add(messageField);
                }
            }

            // Add custom unreal comment fields
            void AddComment(string comment)
            {
                message.UnrealCommentFields.Add(comment.Trim());
            }
            foreach (var location in serviceFile.ProtoFileDesc.SourceCodeInfo.Location)
            {
                if (location.HasLeadingComments)
                {
                    AddComment(location.LeadingComments);
                }
                
                if (location.HasTrailingComments)
                {
                    AddComment(location.TrailingComments);
                }
            }

            message.Index = serviceFile.MessageArray.Count;
            serviceFile.Message2IndexMap.Add(
                "." + serviceFile.PackageName + "." +
                TurboLinkUtils.JoinString(parentMessageNameList, ".") +
                protoMessage.Name,
                message.Index);
            serviceFile.MessageArray.Add(message);
        }

        private void AddServices(string protoFileName)
        {
            var serviceFile = GrpcServiceFiles[protoFileName];
            serviceFile.ServiceArray = new List<GrpcService>();

            foreach (var service in serviceFile.ProtoFileDesc.Service)
            {
                var newService = new GrpcService(service);
                newService.MethodArray = new List<GrpcServiceMethod>();

                foreach (var method in service.Method) newService.MethodArray.Add(new GrpcServiceMethod(method));
                serviceFile.ServiceArray.Add(newService);
            }

            GrpcServiceFiles[protoFileName] = serviceFile;
        }

        private void AnalyzeMessage(string protoFileName)
        {
            var serviceFile = GrpcServiceFiles[protoFileName];

            //find message index that each field directly depends on
            foreach (var message in serviceFile.MessageArray)
            foreach (var messageField in message.Fields)
            {
                if (messageField.FieldDesc == null || //Oneof message field
                    messageField.FieldDesc.Type != FieldDescriptorProto.Types.Type.Message) continue;
                var typeName = messageField.FieldDesc.TypeName;

                if (messageField is GrpcMessageField_Map)
                {
                    //for map field, pick value field name, eg. "map<string, Address>" => "Address"
                    var mapMessageField = (GrpcMessageField_Map)messageField;
                    typeName = mapMessageField.ValueField.FieldDesc.TypeName;
                }

                if (serviceFile.Message2IndexMap.ContainsKey(typeName))
                    if (serviceFile.Message2IndexMap[typeName] >= message.Index)
                    {
                        messageField.NeedNativeMake = true;
                        message.HasNativeMake = true;
                    }
            }
        }
    }
}