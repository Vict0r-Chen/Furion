﻿// 麻省理工学院许可证
//
// 版权所有 © 2020-2023 百小僧，百签科技（广东）有限公司
//
// 特此免费授予获得本软件及其相关文档文件（以下简称“软件”）副本的任何人以处理本软件的权利，
// 包括但不限于使用、复制、修改、合并、发布、分发、再许可、销售软件的副本，
// 以及允许拥有软件副本的个人进行上述行为，但须遵守以下条件：
//
// 在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，
// 无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Furion.OpenApi;

/// <summary>
/// API 描述器解析器
/// </summary>
public sealed class ApiDescriptionParser
{
    /// <inheritdoc cref="IApiDescriptionGroupCollectionProvider"/>
    internal readonly IApiDescriptionGroupCollectionProvider _provider;

    internal readonly JsonOptions _jsonOptions;

    /// <summary>
    /// <inheritdoc cref="ApiDescriptionParser"/>
    /// </summary>
    /// <param name="provider"><see cref="IApiDescriptionGroupCollectionProvider"/></param>
    /// <param name="options"><see cref="IOptions{TOptions}"/></param>
    public ApiDescriptionParser(IApiDescriptionGroupCollectionProvider provider
        , IOptions<JsonOptions> options)
    {
        _provider = provider;
        _jsonOptions = options.Value;
    }

    /// <summary>
    /// 解析 API 描述器并返回开放接口模型
    /// </summary>
    /// <returns><see cref="OpenApiModel"/></returns>
    public OpenApiModel Parse()
    {
        var openApiModel = new OpenApiModel();
        var projectName = Assembly.GetEntryAssembly()?.GetName()?.Name;

        foreach (var group in _provider.ApiDescriptionGroups.Items)
        {
            var openApiGroup = new OpenApiGroup
            {
                Name = group.GroupName ?? projectName
            };

            foreach (var item in group.Items)
            {
                var actionDescriptor = item.ActionDescriptor;
                var controllerName = (actionDescriptor as ControllerActionDescriptor)?.ControllerName ?? actionDescriptor.RouteValues["controller"] ?? projectName;

                var openApiTag = openApiGroup.Tags.FirstOrDefault(u => u.Name == controllerName);
                if (openApiTag is null)
                {
                    openApiTag = new OpenApiTag
                    {
                        Name = controllerName
                    };
                    openApiGroup.Tags.Add(openApiTag);
                }

                var openApiDescription = new OpenApiDescription
                {
                    Id = actionDescriptor.Id,
                    GroupName = item.GroupName ?? projectName,
                    HttpMethod = item.HttpMethod,
                    RelativePath = item.RelativePath,
                    AllowAnonymous = actionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()
                };

                ParseParameters(item.ParameterDescriptions, openApiDescription);

                openApiTag.Descriptions.Add(openApiDescription);
            }

            openApiModel.Groups.Add(openApiGroup);
        }

        return openApiModel;
    }

    public void ParseParameters(IList<ApiParameterDescription> parameterDescriptions, OpenApiDescription openApiDescription)
    {
        if (parameterDescriptions is null or { Count: 0 })
        {
            return;
        }

        var openApiParameters = new List<OpenApiProperty>();
        foreach (var parameterDescription in parameterDescriptions)
        {
            if (parameterDescription.ModelMetadata is null)
            {
                continue;
            }

            if (parameterDescription.Source.Id != "Body" && parameterDescription.ModelMetadata.IsBindingAllowed)
            {
                var openApiParameter = new ModelMetadataParser(parameterDescription.ModelMetadata, parameterDescription);

                openApiParameters.Add(openApiParameter.Parse()!);
            }
            else
            {
                var properties = parameterDescription.ModelMetadata.Properties;

                foreach (var property in properties)
                {
                    if (property.ContainerMetadata?.ModelType == parameterDescription.ModelMetadata.ModelType)
                    {

                    }
                }
            }
        }

        openApiDescription.Parameters = openApiParameters;
    }
}