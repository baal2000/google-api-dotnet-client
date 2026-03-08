/*
Copyright 2026 Google Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Google.Apis.Util
{
    /// <summary>
    /// Provides cached reflection results for request parameter discovery.
    /// </summary>
    /// <remarks>
    /// This cache is only used when <see cref="ReflectionCacheSettings.EnableReflectionCache"/> is set to true.
    /// By default, reflection results are recomputed on each call to avoid memory overhead.
    /// </remarks>
    internal static partial class ReflectionCache
    {
        /// <summary>
        /// Cache of properties filtered by RequestParameterAttribute.
        /// Key is the type, value is an array of PropertyWithAttribute structs.
        /// </summary>
        /// <remarks>
        /// This cache is intentionally unbounded; it is keyed by request types
        /// annotated with RequestParameterAttribute, which are expected to be
        /// a finite set in normal usage.
        /// </remarks>
        private static readonly ConcurrentDictionary<Type, PropertyWithAttribute[]> RequestParameterPropertiesCache =
            new ConcurrentDictionary<Type, PropertyWithAttribute[]>();

        /// <summary>
        /// Returns the set of request-parameter properties for the specified request type.
        /// Uses caching if <see cref="ReflectionCacheSettings.EnableReflectionCache"/> is enabled.
        /// </summary>
        /// <param name="type">The type to get request parameter properties for.</param>
        /// <returns>An array of <see cref="PropertyWithAttribute"/> structs containing properties and their RequestParameterAttribute.</returns>
        internal static PropertyWithAttribute[] GetRequestParameterProperties(Type type)
        {
            // Only use cache if explicitly enabled by user
            if (ReflectionCacheSettings.EnableReflectionCache)
            {
                return RequestParameterPropertiesCache.GetOrAdd(type, ComputeProperties);
            }

            // Default behavior: compute properties without caching
            return ComputeProperties(type);
        }

        /// <summary>
        /// Computes the request parameter properties for a given type using reflection.
        /// </summary>
        private static PropertyWithAttribute[] ComputeProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(prop => new PropertyWithAttribute(prop, prop.GetCustomAttribute<RequestParameterAttribute>(inherit: false)))
                .Where(pwa => pwa.Attribute != null)
                .ToArray();
        }
    }
}
