using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Ak.Framework.Wpf.Extensions;
using Ak.Framework.Wpf.ViewModels;

namespace Ak.Framework.Wpf.Binding
{
    /// <summary>
    /// Extension for working with events binding
    /// Class was taken from https://github.com/JonghoL/EventBindingMarkup
    /// </summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    public class EventBindingExtension : MarkupExtension
    {
        /// <summary>
        /// Command
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Command parameter
        /// </summary>
        public string CommandParameter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (targetProvider == null)
            {
                throw new InvalidOperationException();
            }

            FrameworkElement targetObject = targetProvider.TargetObject as FrameworkElement;
            if (targetObject == null)
            {
                throw new InvalidOperationException();
            }

            MemberInfo memberInfo = targetProvider.TargetProperty as MemberInfo;
            if (memberInfo == null)
            {
                throw new InvalidOperationException();
            }

            if (string.IsNullOrWhiteSpace(Command))
            {
                Command = memberInfo.Name.Replace("Add", "");
                if (Command.Contains("Handler"))
                {
                    Command = Command.Replace("Handler", "Command");
                }
                else
                {
                    Command = Command + "Command";
                }
            }

            return CreateHandler(memberInfo, Command, targetObject.GetType());
        }

        private Type GetEventHandlerType(MemberInfo memberInfo)
        {
            Type eventHandlerType = null;
            if (memberInfo is EventInfo)
            {
                EventInfo info = memberInfo as EventInfo;
                EventInfo eventInfo = info;
                eventHandlerType = eventInfo.EventHandlerType;
            }
            else if (memberInfo is MethodInfo)
            {
                MethodInfo info = memberInfo as MethodInfo;
                MethodInfo methodInfo = info;
                ParameterInfo[] pars = methodInfo.GetParameters();
                eventHandlerType = pars[1].ParameterType;
            }

            return eventHandlerType;
        }

        private object CreateHandler(MemberInfo memberInfo, string cmdName, Type targetType)
        {
            Type eventHandlerType = GetEventHandlerType(memberInfo);

            if (eventHandlerType == null) return null;

            MethodInfo handlerInfo = eventHandlerType.GetMethod("Invoke");
            DynamicMethod method = new DynamicMethod("", handlerInfo.ReturnType,
                new Type[]
                {
                    handlerInfo.GetParameters()[0].ParameterType,
                    handlerInfo.GetParameters()[1].ParameterType,
                });

            ILGenerator gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldarg, 0);
            gen.Emit(OpCodes.Ldarg, 1);
            gen.Emit(OpCodes.Ldstr, cmdName);
            if (CommandParameter == null)
            {
                gen.Emit(OpCodes.Ldnull);
            }
            else
            {
                gen.Emit(OpCodes.Ldstr, CommandParameter);
            }
            gen.Emit(OpCodes.Call, getMethod);
            gen.Emit(OpCodes.Ret);

            return method.CreateDelegate(eventHandlerType);
        }

        static readonly MethodInfo getMethod = typeof(EventBindingExtension).GetMethod("HandlerIntern", new Type[] { typeof(object), typeof(object), typeof(string), typeof(string) });

        static void Handler(object sender, object args)
        {
            HandlerIntern(sender, args, "cmd", null);
        }

        public static void HandlerIntern(object sender, object args, string cmdName, string commandParameter)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe != null)
            {
                ICommand cmd = GetCommand(fe, cmdName);
                object commandParam = null;
                if (!string.IsNullOrWhiteSpace(commandParameter))
                {
                    commandParam = GetCommandParameter(fe, args, commandParameter);
                }
                if ((cmd != null) && cmd.CanExecute(commandParam))
                {
                    cmd.Execute(commandParam);
                }
            }
        }

        internal static ICommand GetCommand(FrameworkElement target, string cmdName)
        {
            ViewModelBase vm = FindViewModel(target);
            if (vm == null) return null;

            Type vmType = vm.GetType();
            PropertyInfo cmdProp = vmType.GetProperty(cmdName);
            if (cmdProp != null)
            {
                return cmdProp.GetValue(vm) as ICommand;
            }
#if DEBUG
            throw new Exception("EventBinding path error: '" + cmdName + "' property not found on '" + vmType + "' 'DelegateCommand'");
#endif

            return null;
        }

        internal static object GetCommandParameter(FrameworkElement target, object args, string commandParameter)
        {
            object ret = null;
            string[] classify = commandParameter.Split('.');
            switch (classify[0])
            {
                case "$e":
                    ret = args;
                    break;
                case "$this":
                    ret = classify.Length > 1 ? FollowPropertyPath(target, commandParameter.Replace("$this.", ""), target.GetType()) : target;
                    break;
                default:
                    ret = commandParameter;
                    break;
            }

            return ret;
        }

        internal static ViewModelBase FindViewModel(FrameworkElement target)
        {
            if (target == null)
                return null;

            if (target.DataContext is ViewModelBase vm)
                return vm;

            FrameworkElement parent = target.GetParentObject() as FrameworkElement;

            return FindViewModel(parent);
        }

        internal static object FollowPropertyPath(object target, string path, Type valueType = null)
        {
            if (target == null) throw new ArgumentNullException("target null");
            if (path == null) throw new ArgumentNullException("path null");

            Type currentType = valueType ?? target.GetType();

            foreach (string propertyName in path.Split('.'))
            {
                PropertyInfo property = currentType.GetProperty(propertyName);
                if (property == null) throw new NullReferenceException("property null");

                target = property.GetValue(target);
                currentType = property.PropertyType;
            }
            return target;
        }
    }
}
