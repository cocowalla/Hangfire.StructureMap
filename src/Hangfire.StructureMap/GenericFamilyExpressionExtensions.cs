namespace Hangfire.StructureMap
{
    using global::StructureMap.Configuration.DSL.Expressions;

    /// <summary>
    /// Generic Family Expression Extensions.
    /// </summary>
    public static class GenericFamilyExpressionExtensions
    {
        /// <summary>
        /// Convenience method to mark a PluginFamily as BackgroundJob scoped.
        /// </summary>
        /// <param name="expression">The <see cref="GenericFamilyExpression"/> expression.</param>
        /// <returns>The <see cref="GenericFamilyExpression"/> marked as <see cref="BackgroundJobLifecycle"/> scoped.</returns>
        public static GenericFamilyExpression BackgroundJobScoped(this GenericFamilyExpression expression)
        {
            return expression.LifecycleIs(new BackgroundJobLifecycle());
        }
    }
}
