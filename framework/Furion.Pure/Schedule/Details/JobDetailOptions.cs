﻿// MIT License
//
// Copyright (c) 2020-2022 百小僧, Baiqian Co.,Ltd and Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace Furion.Schedule;

/// <summary>
/// 作业信息配置选项
/// </summary>
[SuppressSniffer]
public sealed class JobDetailOptions
{
    /// <summary>
    /// 构造函数
    /// </summary>
    internal JobDetailOptions()
    {
    }

    /// <summary>
    /// 重写 <see cref="ConvertToSQL"/>
    /// </summary>
    public Func<string, string[], JobDetail, PersistenceBehavior, NamingConventions, string> ConvertToSQL
    {
        get
        {
            return ConvertToSQLConfigure;
        }
        set
        {
            ConvertToSQLConfigure = value;
        }
    }

    /// <summary>
    /// 启用作业执行详细日志
    /// </summary>
    public bool LogEnabled
    {
        get
        {
            return InternalLogEnabled;
        }
        set
        {
            InternalLogEnabled = value;
        }
    }

    /// <summary>
    /// <see cref="LogEnabled"/> 静态配置
    /// </summary>
    internal static bool InternalLogEnabled { get; private set; }

    /// <summary>
    /// <see cref="ConvertToSQL"/> 静态配置
    /// </summary>
    internal static Func<string, string[], JobDetail, PersistenceBehavior, NamingConventions, string> ConvertToSQLConfigure { get; private set; }
}