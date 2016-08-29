using Invert.Core.GraphDesigner;
using System;
using System.CodeDom;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace uFrame.MVVM.Templates
{
    public class uFrameBindingType
    {
        private string _displayFormat = "{0}";

        public Action<BindingHandlerArgs> HandlerImplementation;

        public string DisplayFormat
        {
            get
            {
                return this._displayFormat;
            }
            set
            {
                this._displayFormat = value;
            }
        }

        public string Description;

        public Type Type;

        public MethodInfo MethodInfo;

        public Func<ITypedItem, bool> CanBind;

        public static Type ObservablePropertyType;

        public static Type ObservableCollectionType;

        public static Type UFGroupType;

        public static Type ICommandType;

        public uFrameBindingType SetNameFormat(string format)
        {
            this.DisplayFormat = format;
            return this;
        }

        public uFrameBindingType SetDescription(string description)
        {
            this.Description = description;
            return this;
        }

        public uFrameBindingType ImplementWith(Action<BindingHandlerArgs> implement)
        {
            this.HandlerImplementation = implement;
            return this;
        }

        public uFrameBindingType(Type type, string methodFormat, Func<ITypedItem, bool> canBind)
        {
            this.Type = type;
            this.CanBind = canBind;
            this.DisplayFormat = methodFormat;
            this.MethodInfo = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                                  .FirstOrDefault((MethodInfo p) => !p.IsDefined(typeof(ObsoleteAttribute), true) && p.Name == methodFormat);
            if (this.MethodInfo == null)
            {
                throw new Exception(string.Format("Couldn't register binding for method {0}.{1} because it was not found", type.Name, methodFormat));
            }
        }

        public uFrameBindingType(Type type, MethodInfo methodInfo, Func<ITypedItem, bool> canBind)
        {
            this.Type = type;
            this.MethodInfo = methodInfo;
            this.CanBind = canBind;
            this.DisplayFormat = methodInfo.Name;
        }

        public CodeExpression CreateBindingSignature(CreateBindingSignatureParams createBindingSignatureParams)
        {
            string name = createBindingSignatureParams.ElementView.Element.Name;
            string text = string.Format(createBindingSignatureParams.SubscribablePropertyNameFormat, createBindingSignatureParams.SourceItem.Name);
            string name2 = createBindingSignatureParams.SourceItem.Name;
            CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), this.MethodInfo.Name, new CodeExpression[0]);
            int num;
            for (int i = 0; i < this.MethodInfo.GetParameters().Length; i = num + 1)
            {
                ParameterInfo parameterInfo = this.MethodInfo.GetParameters()[i];
                if (!(this.MethodInfo.IsDefined(typeof(ExtensionAttribute), true) && i == 0))
                {
                    Type[] genericArguments = parameterInfo.ParameterType.GetGenericArguments();
                    if (typeof(Delegate).IsAssignableFrom(parameterInfo.ParameterType))
                    {
                        CodeMemberMethod codeMemberMethod = this.CreateDelegateMethod(createBindingSignatureParams.ConvertGenericParameter, parameterInfo, genericArguments, text, name2);
                        codeMethodInvokeExpression.Parameters.Add(new CodeSnippetExpression(string.Format("this.{0}", codeMemberMethod.Name)));
                        createBindingSignatureParams.Context.Members.Add(codeMemberMethod);
                        if (this.HandlerImplementation != null)
                        {
                            this.HandlerImplementation(new BindingHandlerArgs
                            {
                                View = createBindingSignatureParams.ElementView,
                                SourceItem = createBindingSignatureParams.SourceItem,
                                Method = codeMemberMethod,
                                Decleration = createBindingSignatureParams.Context,
                                IsDesignerFile = !createBindingSignatureParams.DontImplement
                            });
                        }
                        bool dontImplement = createBindingSignatureParams.DontImplement;
                        if (dontImplement)
                        {
                            CodeMemberMethod codeMemberMethod2 = codeMemberMethod;
                            codeMemberMethod2.Attributes |= MemberAttributes.Override;
                        }
                        createBindingSignatureParams.Ctx.AddMemberOutput(createBindingSignatureParams.BindingsReference, new TemplateMemberResult(null, null, new GenerateMethod(TemplateLocation.Both), codeMemberMethod, createBindingSignatureParams.Ctx.CurrentDeclaration));
                    }
                    else
                    {
                        if (typeof(ICollection).IsAssignableFrom(parameterInfo.ParameterType))
                        {
                            codeMethodInvokeExpression.Parameters.Add(new CodeSnippetExpression(string.Format("this.{0}.{1}", name, createBindingSignatureParams.SourceItem.Name)));
                        }
                        else
                        {
                            if (uFrameBindingType.ObservablePropertyType.IsAssignableFrom(parameterInfo.ParameterType))
                            {
                                codeMethodInvokeExpression.Parameters.Add(new CodeSnippetExpression(string.Format("this.{0}.{1}", name, text)));
                            }
                            else
                            {
                                if (uFrameBindingType.ICommandType.IsAssignableFrom(parameterInfo.ParameterType))
                                {
                                    codeMethodInvokeExpression.Parameters.Add(new CodeSnippetExpression(string.Format("this.{0}.{1}", name, createBindingSignatureParams.SourceItem.Name)));
                                }
                                else
                                {
                                    if (!createBindingSignatureParams.DontImplement)
                                    {
                                        string text2 = parameterInfo.Name.Substring(0, 1).ToUpper() + parameterInfo.Name.Substring(1);
                                        CodeMemberField codeMemberField = createBindingSignatureParams.Context._protected_(parameterInfo.ParameterType, "_{0}{1}", new object[]
										{
											name2,
											text2
										});
                                        codeMemberField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(uFrameBindingType.UFGroupType), new CodeAttributeArgument[]
										{
											new CodeAttributeArgument(new CodePrimitiveExpression(name2))
										}));
                                        codeMemberField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializeField))));
                                        codeMemberField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(HideInInspector))));
                                        codeMethodInvokeExpression.Parameters.Add(new CodeSnippetExpression(codeMemberField.Name));
                                        codeMemberField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(FormerlySerializedAsAttribute)), new CodeAttributeArgument[]
										{
											new CodeAttributeArgument(new CodePrimitiveExpression(string.Format("_{0}{1}", name2, parameterInfo.Name)))
										}));
                                    }
                                }
                            }
                        }
                    }
                }
                num = i;
            }
            return codeMethodInvokeExpression;
        }

        public CodeMemberMethod CreateDelegateMethod(Func<Type, CodeTypeReference> convertGenericParameter, ParameterInfo parameter, Type[] genericArguments, string propertyName, string name)
        {
            CodeMemberMethod codeMemberMethod = new CodeMemberMethod
            {
                Name = string.Format("{0}{1}{2}", name, parameter.Name.Substring(0, 1).ToUpper(), parameter.Name.Substring(1)),
                Attributes = MemberAttributes.Public
            };
            if (parameter.ParameterType.Name.Contains("Func"))
            {
                Type type = genericArguments.LastOrDefault<Type>();
                if (type != null)
                {
                    codeMemberMethod.ReturnType = new CodeTypeReference(type);
                }
            }
            int num = 1;
            for (int i = 0; i < genericArguments.Length; i++)
            {
                Type type2 = genericArguments[i];
                if (!(parameter.ParameterType.Name.Contains("Func") && type2 == genericArguments.Last<Type>()))
                {
                    Type type3 = type2;
                    bool isGenericParameter = type2.IsGenericParameter;
                    if (isGenericParameter)
                    {
                        codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(convertGenericParameter(type2), string.Format("arg{0}", num)));
                    }
                    else
                    {
                        codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(type3, string.Format("arg{0}", num)));
                    }
                }
            }
            return codeMemberMethod;
        }

        public static void CreateActionSignature(Type actionType)
        {
        }
    }
}
