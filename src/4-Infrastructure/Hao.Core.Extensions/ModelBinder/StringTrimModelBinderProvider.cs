using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Hao.Core.Extensions
{
    public class StringTrimModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.UnderlyingOrModelType == typeof(string))
            {
                return new StringTrimModelBinder();
            }
            return null;
        }
    }



    /// <summary>
    /// 提供对字符串前后空白进行Trim操作的模型绑定能力
    /// </summary>
    public class StringTrimModelBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            Type underlyingOrModelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try
            {
                string firstValue = valueProviderResult.FirstValue;
                object model;
                if (string.IsNullOrWhiteSpace(firstValue))
                {
                    model = null;
                }
                else
                {
                    if (underlyingOrModelType != typeof(string))
                    {
                        throw new MulticastNotSupportedException();
                    }
                    model = firstValue.Trim();
                }
                bindingContext.Result = ModelBindingResult.Success(model);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                if (!(exception is FormatException) && exception.InnerException != null)
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, exception, bindingContext.ModelMetadata);
                return Task.CompletedTask;
            }
        }
    }
}
