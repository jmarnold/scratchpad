using NUnit.Framework;
using Rhino.Mocks;
using StructureMap;
using StructureMap.AutoMocking;

namespace Scratchpad
{
    /// <summary>
    /// Provides a context for all interaction tests.
    /// </summary>
    /// <typeparam name="TClassToTest">The class for which the interaction tests are created.</typeparam>
    public class InteractionContext<TClassToTest> 
        where TClassToTest : class 
    {
        private readonly MockMode _mode;
        private RhinoAutoMocker<TClassToTest> _services;
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionContext{T}"/> class.
        /// </summary>
        public InteractionContext()
            : this(MockMode.AAA)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionContext{T}"/> class and instructs
        /// the underlying <see cref="RhinoAutoMocker{TARGETCLASS}"/> to use the specified mocking mode.
        /// </summary>
        private InteractionContext(MockMode mode)
        {
            _mode = mode;
        }
        /// <summary>
        /// Gets the underlying <see cref="RhinoAutoMocker{TARGETCLASS}"/>.
        /// </summary>
        public RhinoAutoMocker<TClassToTest> Services
        {
            get { return _services; }
        }
        /// <summary>
        /// Gets the <see cref="IContainer"/> controlling the context.
        /// </summary>
        public IContainer Container
        {
            get { return Services.Container; }
        }
        /// <summary>
        /// Gets the instance of the class being testing.
        /// </summary>
        public TClassToTest ClassUnderTest
        {
            get { return _services.ClassUnderTest; }
        }
        /// <summary>
        /// Returns the current mocked instance for the specified <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of mocked instance to retrieve.</typeparam>
        /// <returns></returns>
        public TService MockFor<TService>() 
            where TService : class
        {
            return _services.Get<TService>();
        }
        /// <summary>
        /// Verifies all expectations for the specified <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of mocked instance to verify.</typeparam>
        public void VerifyCallsFor<TService>() 
            where TService : class
        {
            MockFor<TService>().VerifyAllExpectations();
        }
        /// <summary>
        /// Called by the NUnit framework to perform setup tasks.
        /// Context-specific setup tasks should be implemented by overriding the <see cref="beforeEach"/> method.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _services = new RhinoAutoMocker<TClassToTest>(_mode);

            beforeEach();
        }
        /// <summary>
        /// Called after setup tasks are performed in the <see cref="SetUp"/> method.
        /// </summary>
        protected virtual void beforeEach()
        {
        }
    }
}