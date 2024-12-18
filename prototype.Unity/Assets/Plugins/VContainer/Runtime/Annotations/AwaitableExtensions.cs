//
// Copyright (c) 2024 left (https://github.com/left-/)
// Copyright (c) 2020 hadashiA
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
//

#if UNITY_2021_3_OR_NEWER
#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using System;
using Awaitable = System.Threading.Tasks.Task;
#endif

namespace VContainer.Unity
{
    internal static class AwaitableExtensions
    {
        public static async Awaitable Forget(this Awaitable awaitable,
            EntryPointExceptionHandler exceptionHandler = null)
        {
            try
            {
                await awaitable;
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                    exceptionHandler.Publish(ex);
                else
                    throw;
            }
        }
    }
}
#endif