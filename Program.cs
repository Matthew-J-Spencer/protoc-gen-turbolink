﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;

namespace protoc_gen_turbolink
{
    class Program
    {
        static void Main(string[] args)
        {
            //read code generator request from stdin.
            CodedInputStream inputStream = new CodedInputStream(Console.OpenStandardInput());
            CodeGeneratorRequest request = new CodeGeneratorRequest();
            request.MergeFrom(inputStream);

            //create code generator reponse
            CodeGeneratorResponse response = new CodeGeneratorResponse();

            foreach (FileDescriptorProto protoFile in request.ProtoFile)
            {
                TurboLinkGenerator generator = new TurboLinkGenerator(protoFile);

                generator.Prepare();
                generator.BuildOutputFiles();

                foreach (GeneratedFile generatedFile in generator.GeneratedFiles)
                {
                    CodeGeneratorResponse.Types.File newFile = new CodeGeneratorResponse.Types.File();

                    newFile.Name = generatedFile.FileName;
                    newFile.Content = generatedFile.Content;
                    response.File.Add(newFile);
                }
            }

            //debug file
            //CodeGeneratorResponse.Types.File file = new CodeGeneratorResponse.Types.File();
            //file.Name = "request_dump.json";
            //file.Content = string.Format("{0}", request.ToString());
            //response.File.Add(file);

            //supported features(optional field)
            response.SupportedFeatures = (ulong)CodeGeneratorResponse.Types.Feature.Proto3Optional;

            //write response from standard output to grpc
            byte[] data = new byte[response.CalculateSize()];
            Google.Protobuf.CodedOutputStream outputStream = new Google.Protobuf.CodedOutputStream(data);
            response.WriteTo(outputStream);

            Console.OpenStandardOutput().Write(data, 0, response.CalculateSize());
        }
    }
}
