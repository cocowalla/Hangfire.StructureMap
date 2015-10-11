namespace Hangfire.StructureMap
{
    using System;

    using global::StructureMap;
    using global::StructureMap.Pipeline;

    /// <summary>
    /// Background Job Lifecycle
    /// </summary>
    public class BackgroundJobLifecycle : LifecycleBase
    {
        private static readonly Lazy<IObjectCache> Cache = new Lazy<IObjectCache>(() => new LifecycleObjectCache());

        /// <summary>
        /// Disposes and clears all the cache items.
        /// </summary>
        public static void DisposeAndClearAll()
        {
            Cache.Value.DisposeAndClear();
        }

        /// <inheritdoc />
        public override void EjectAll(ILifecycleContext context)
        {
            FindCache(context).DisposeAndClear();
        }

        /// <inheritdoc />
        public override IObjectCache FindCache(ILifecycleContext context)
        {
            return Cache.Value;
        }
    }
}
