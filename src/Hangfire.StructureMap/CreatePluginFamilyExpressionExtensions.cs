namespace Hangfire.StructureMap
{
    using global::StructureMap.Configuration.DSL.Expressions;

    /// <summary>
    /// Create Plugin Family Expression Extensions.
    /// </summary>
    public static class CreatePluginFamilyExpressionExtensions
    {
        /// <summary>
        /// Convenience method to mark a PluginFamily as BackgroundJob scoped.
        /// </summary>
        /// <typeparam name="TPluginType">The type of the plugin type.</typeparam>
        /// <param name="source">The source expression.</param>
        /// <returns>The <see cref="CreatePluginFamilyExpression{TPluginType}"/> marked as <see cref="BackgroundJobLifecycle"/> scoped.</returns>
        public static CreatePluginFamilyExpression<TPluginType> BackgroundJobScoped<TPluginType>(this CreatePluginFamilyExpression<TPluginType> source)
        {
            return source.LifecycleIs<BackgroundJobLifecycle>();
        }
    }
}
