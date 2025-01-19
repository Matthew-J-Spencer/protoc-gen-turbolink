﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace protoc_gen_turbolink.Template
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Google.Protobuf.Reflection;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class NodeCPP : NodeCPPBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("//Generated by TurboLink CodeGenerator, do not edit!\r\n#include \"");
            
            #line 8 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(s.TurboLinkBasicFileName));
            
            #line default
            #line hidden
            this.Write("Node.h\"\r\n#include \"");
            
            #line 9 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(s.TurboLinkBasicFileName));
            
            #line default
            #line hidden
            this.Write("Service.h\"\r\n#include \"TurboLinkGrpcManager.h\"\r\n#include \"TurboLinkGrpcUtilities.h" +
                    "\"\r\n#include \"Engine/World.h\"\r\n#include \"TimerManager.h\"\r\n#include \"Runtime/Launc" +
                    "h/Resources/Version.h\"\r\n");
            
            #line 15 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"

foreach(GrpcService service in s.ServiceArray)
{
	foreach (GrpcServiceMethod method in service.MethodArray)
	{
		if(!method.ClientStreaming) {

            
            #line default
            #line hidden
            this.Write("\r\nUCall");
            
            #line 23 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 23 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("* UCall");
            
            #line 23 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 23 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::");
            
            #line 23 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("(UObject* WorldContextObject, const ");
            
            #line 23 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.InputType));
            
            #line default
            #line hidden
            this.Write("& request, FGrpcMetaData metaData, float deadLineSeconds)\r\n{\r\n\tUCall");
            
            #line 25 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 25 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("* node = NewObject<UCall");
            
            #line 25 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 25 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write(">(WorldContextObject);\r\n\tUTurboLinkGrpcManager* turboLinkManager = UTurboLinkGrpc" +
                    "Utilities::GetTurboLinkGrpcManager(WorldContextObject);\r\n\r\n\tnode->");
            
            #line 28 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write(" = Cast<U");
            
            #line 28 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write(">(turboLinkManager->MakeService(\"");
            
            #line 28 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("\"));\r\n\tif (node->");
            
            #line 29 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write(" == nullptr)\r\n\t{\r\n\t\treturn nullptr;\r\n\t}\r\n\tnode->ServiceState = EGrpcServiceState:" +
                    ":Idle;\r\n\tnode->Request = request;\r\n\tnode->MetaData = metaData;\r\n\tnode->DeadLineS" +
                    "econds = deadLineSeconds;\r\n\r\n\tnode->");
            
            #line 38 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("->OnServiceStateChanged.AddUniqueDynamic(node, &UCall");
            
            #line 38 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 38 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::OnServiceStateChanged);\r\n\treturn node;\r\n}\r\n\r\nvoid UCall");
            
            #line 42 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 42 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::Activate()\r\n{\r\n\t");
            
            #line 44 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("->Connect();\r\n}\r\n\r\nvoid UCall");
            
            #line 47 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 47 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write(@"::OnServiceStateChanged(EGrpcServiceState NewState)
{
	if (ServiceState == NewState) return;
	ServiceState = NewState;

	if (NewState == EGrpcServiceState::TransientFailure)
	{
		FGrpcResult result;
		result.Code = EGrpcResultCode::ConnectionFailed;

		");
            
            #line 57 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.OutputType));
            
            #line default
            #line hidden
            this.Write(" response;\r\n\t\tOnFail.Broadcast(result, response);\r\n\r\n\t\tShutdown();\r\n\t\treturn;\r\n\t}" +
                    "\r\n\r\n\tif (NewState == EGrpcServiceState::Ready)\r\n\t{\r\n\t\t");
            
            #line 66 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client = ");
            
            #line 66 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("->MakeClient();\r\n\t\t");
            
            #line 67 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client->OnContextStateChange.AddUniqueDynamic(this, &UCall");
            
            #line 67 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 67 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::OnContextStateChange);\r\n\t\t");
            
            #line 68 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client->On");
            
            #line 68 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("Response.AddUObject(this, &UCall");
            
            #line 68 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 68 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::OnResponse);\r\n\r\n\t\tContext = ");
            
            #line 70 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client->Init");
            
            #line 70 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("();\r\n\t\t");
            
            #line 71 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client->");
            
            #line 71 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("(Context, Request, MetaData, DeadLineSeconds);\r\n\t}\r\n}\r\n\r\nvoid UCall");
            
            #line 75 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 75 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::OnContextStateChange(FGrpcContextHandle Handle, EGrpcContextState State)\r\n{\r\n\ti" +
                    "f (State == EGrpcContextState::Done)\r\n\t{\r\n");
            
            #line 79 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
if(method.ServerStreaming) {
            
            #line default
            #line hidden
            this.Write("\t\tOnFinished.Broadcast(FGrpcResult{}, ");
            
            #line 80 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.OutputType));
            
            #line default
            #line hidden
            this.Write("{});\r\n");
            
            #line 81 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
}
            
            #line default
            #line hidden
            this.Write("\t\tShutdown();\r\n\t}\r\n}\r\n\r\nvoid UCall");
            
            #line 86 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 86 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::OnResponse(FGrpcContextHandle Handle, const FGrpcResult& GrpcResult, const ");
            
            #line 86 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.OutputType));
            
            #line default
            #line hidden
            this.Write("& Response)\r\n{\r\n\tif (GrpcResult.Code == EGrpcResultCode::Ok)\r\n\t{\r\n\t\tOn");
            
            #line 90 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("Response.Broadcast(GrpcResult, Response);\r\n\t}\r\n\telse\r\n\t{\r\n\t\tOnFail.Broadcast(Grpc" +
                    "Result, Response);\r\n\t}\r\n}\r\n\r\nvoid UCall");
            
            #line 98 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 98 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::Shutdown()\r\n{\r\n\t");
            
            #line 100 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("->OnServiceStateChanged.RemoveDynamic(this, &UCall");
            
            #line 100 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            
            #line 100 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("::OnServiceStateChanged);\r\n\tif (");
            
            #line 101 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client != nullptr)\r\n\t{\r\n\t\t");
            
            #line 103 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("->RemoveClient(");
            
            #line 103 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client);\r\n\t\t");
            
            #line 104 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client->Shutdown();\r\n\t\t");
            
            #line 105 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write("Client = nullptr;\r\n\t}\r\n\r\n\tif (");
            
            #line 108 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write(" != nullptr)\r\n\t{\r\n\t\tUTurboLinkGrpcUtilities::GetTurboLinkGrpcManager(this)->Relea" +
                    "seService(");
            
            #line 110 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write(");\r\n\t\t");
            
            #line 111 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(service.Name));
            
            #line default
            #line hidden
            this.Write(" = nullptr;\r\n\t}\r\n\r\n\tSetReadyToDestroy();\r\n#if ENGINE_MAJOR_VERSION>=5\r\n\tMarkAsGar" +
                    "bage();\r\n#else\r\n\tMarkPendingKill();\r\n#endif\r\n}\r\n");
            
            #line 121 "F:\Git\protoc-gen-turbolink\Template\NodeCPP.tt"

		}
	}
}

            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class NodeCPPBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        public System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
