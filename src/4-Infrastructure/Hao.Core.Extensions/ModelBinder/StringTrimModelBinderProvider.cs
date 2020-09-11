using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Hao.Core.Extensions
{
    public class StringTrimModelBinderProvider : IModelBinderProvider
    {
        private readonly IList<IInputFormatter> _formatters;

        public StringTrimModelBinderProvider(IList<IInputFormatter> formatters)
        {
            _formatters = formatters;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (!context.Metadata.IsComplexType && context.Metadata.UnderlyingOrModelType == typeof(string))
            {
                return new QueryStringTrimModelBinder();
            }
            else if (context.BindingInfo.BindingSource != null && context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Body))
            {
                //通过[FromBody]绑定的
                return new BodyStringTrimModelBinder(_formatters, context.Services.GetRequiredService<IHttpRequestStreamReaderFactory>());
            }

            return null;
        }
    }


    public class QueryStringTrimModelBinder : IModelBinder
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


    public class BodyStringTrimModelBinder : IModelBinder
    {
        private readonly BodyModelBinder bodyModelBinder;

        public BodyStringTrimModelBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
        {
            bodyModelBinder = new BodyModelBinder(formatters, readerFactory);
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            //调用原始body绑定数据
            bodyModelBinder.BindModelAsync(bindingContext);
            //判断是否设置了值
            if (!bindingContext.Result.IsModelSet)
            {
                return Task.CompletedTask;
            }
            //获取绑定对象
            var model = bindingContext.Result.Model;

            /*通过反射修改值,
            也可以实现 IInputFormatter接口里面的ReadAsync方法,自己从Request.Body里面获取数据进行处理,但是那样考虑的比较多也比较复杂,原谅我能力有限。。*/
            var stringPropertyInfo = model.GetType().GetProperties().Where(c => c.PropertyType == typeof(string));
            foreach (PropertyInfo property in stringPropertyInfo)
            {
                string value = property.GetValue(model)?.ToString()?.Trim();
                property.SetValue(model, value);
            }
            return Task.CompletedTask;
        }
    }
}
