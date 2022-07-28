﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TauCode.WebApi.Client.Exceptions;

namespace TauCode.WebApi.Client
{
    public static class ServiceClientExtensions
    {
        #region Private

        private static async Task<Exception> GetFailureException(HttpResponseMessage failMessage)
        {
            try
            {
                var content = await failMessage.Content.ReadAsStringAsync();
                var payloadType = failMessage.Headers.TryGetSingleHeader(DtoHelper.PayloadTypeHeaderName);

                Exception ex = null;

                if (payloadType == DtoHelper.ErrorPayloadType)
                {
                    var error = JsonConvert.DeserializeObject<ErrorDto>(content);

                    switch (failMessage.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            ex = new BadRequestErrorServiceClientException(error.Code, error.Message);
                            break;

                        case HttpStatusCode.Conflict:
                            ex = new ConflictErrorServiceClientException(error.Code, error.Message);
                            break;

                        case HttpStatusCode.Forbidden:
                            ex = new ForbiddenErrorServiceClientException(error.Code, error.Message);
                            break;

                        case HttpStatusCode.NotFound:
                            ex = new NotFoundErrorServiceClientException(error.Code, error.Message);
                            break;

                        default:
                            ex = new ErrorServiceClientException(failMessage.StatusCode, error.Code, error.Message);
                            break;
                    }
                }
                else if (payloadType == DtoHelper.ValidationErrorPayloadType)
                {
                    var validationError = JsonConvert.DeserializeObject<ValidationErrorDto>(content);

                    if (failMessage.StatusCode == HttpStatusCode.BadRequest)
                    {
                        ex = new ValidationErrorServiceClientException(
                            validationError.Code,
                            validationError.Message,
                            validationError.Failures);
                    }
                    else
                    {
                        // actually, something is wrong.
                        ex = new ErrorServiceClientException(
                            failMessage.StatusCode,
                            validationError.Code,
                            validationError.Message);
                    }
                }
                else
                {
                    // Generic error.
                    ex = new HttpServiceClientException(failMessage.StatusCode, content);
                }

                return ex;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        private static string TryGetSingleHeader(this HttpHeaders headers, string headerName)
        {
            var gotHeader = headers.TryGetValues(headerName, out var values);
            if (!gotHeader)
            {
                return null;
            }

            var valueArray = values as string[] ?? values.ToArray();
            return valueArray.Length == 1 ? valueArray[0] : null;
        }

        private static async Task SendAsyncAndHandleResponse(
            this IServiceClient serviceClient,
            HttpMethod method,
            string routeTemplate,
            object segments,
            object queryParams,
            object body,
            IDictionary<string, string> headers,
            CancellationToken cancellationToken)
        {
            var response = await serviceClient.SendAsync(
                method,
                routeTemplate,
                segments,
                queryParams,
                body,
                headers,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var ex = await GetFailureException(response);
                await Task.FromException(ex);
            }
        }

        private static async Task<TResult> SendAsyncAndHandleResponse<TResult>(
            this IServiceClient serviceClient,
            HttpMethod method,
            string routeTemplate,
            object segments,
            object queryParams,
            object body,
            IDictionary<string, string> headers,
            CancellationToken cancellationToken)
        {
            var response = await serviceClient.SendAsync(
                method,
                routeTemplate,
                segments,
                queryParams,
                body,
                headers,
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                try
                {
                    var content = JsonConvert.DeserializeObject<TResult>(json);
                    return content;
                }
                catch (Exception ex)
                {
                    return await Task.FromException<TResult>(ex);
                }
            }
            else
            {
                var ex = await GetFailureException(response);
                return await Task.FromException<TResult>(ex);
            }
        }

        #endregion

        public static Task<TResult> GetAsync<TResult>(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default) =>
            serviceClient.SendAsyncAndHandleResponse<TResult>(
                HttpMethod.Get,
                routeTemplate,
                segments,
                queryParams,
                null,
                headers,
                cancellationToken);

        public static Task PostAsync(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            object body = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default) =>
            serviceClient.SendAsyncAndHandleResponse(
                HttpMethod.Post,
                routeTemplate,
                segments,
                queryParams,
                body,
                headers,
                cancellationToken);

        public static Task<TResult> PostAsync<TResult>(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            object body = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default) =>
            serviceClient.SendAsyncAndHandleResponse<TResult>(
                HttpMethod.Post,
                routeTemplate,
                segments,
                queryParams,
                body,
                headers,
                cancellationToken);

        public static Task PutAsync(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            object body = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default) =>
            serviceClient.SendAsyncAndHandleResponse(
                HttpMethod.Put,
                routeTemplate,
                segments,
                queryParams,
                body,
                headers,
                cancellationToken);

        public static Task<TResult> PutAsync<TResult>(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            object body = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default) =>
            serviceClient.SendAsyncAndHandleResponse<TResult>(
                HttpMethod.Put,
                routeTemplate,
                segments,
                queryParams,
                body,
                headers,
                cancellationToken);

        public static Task DeleteAsync(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default) =>
            serviceClient.SendAsyncAndHandleResponse(
                HttpMethod.Delete,
                routeTemplate,
                segments,
                queryParams,
                null,
                headers,
                cancellationToken);

        public static Task<TResult> DeleteAsync<TResult>(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default) =>
            serviceClient.SendAsyncAndHandleResponse<TResult>(
                HttpMethod.Delete,
                routeTemplate,
                segments,
                queryParams,
                null,
                headers,
                cancellationToken);

        public static async Task<string> DeleteAndReturnIdAsync(
            this IServiceClient serviceClient,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
        {
            var response = await serviceClient.SendAsync(
                HttpMethod.Delete,
                routeTemplate,
                segments,
                queryParams, null,
                headers,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var ex = await GetFailureException(response);
                await Task.FromException(ex);
            }

            var deletedId = response.Headers.TryGetSingleHeader(DtoHelper.DeletedInstanceIdHeaderName);
            return deletedId;
        }
    }
}
