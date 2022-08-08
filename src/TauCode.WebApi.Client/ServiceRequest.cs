﻿namespace TauCode.WebApi.Client;

public class ServiceRequest
{
    public ServiceRequest(HttpMethod httpMethod)
    {
        this.HttpMethod = httpMethod;
    }

    public HttpMethod HttpMethod { get; }

    public string Route { get; set; }

    public IDictionary<string, object> QueryParameters { get; set; }

    public object Body { get; set; }

    public IDictionary<string, string> Headers { get; set; }
}