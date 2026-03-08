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

namespace Google.Apis.Util
{
    /// <summary>
    /// Configuration settings for reflection caching behavior.
    /// </summary>
    public static class ReflectionCacheSettings
    {
        /// <summary>
        /// Gets or sets whether to enable reflection result caching for request parameters.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When enabled, PropertyInfo objects for request parameter properties are cached,
        /// eliminating repeated reflection overhead for the same request types.
        /// </para>
        /// <para>
        /// This should be set once at application startup before making any API requests.
        /// </para>
        /// <para>
        /// Default is <c>false</c> to preserve existing behavior. Enable this if you are making
        /// many requests with the same request types and reflection overhead is a bottleneck.
        /// </para>
        /// </remarks>
        public static bool EnableReflectionCache { get; set; }
    }
}
