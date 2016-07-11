using uFrame.Attributes;

namespace uFrame.ECS.APIs
{
    /// <summary>
    /// This interface, when added to a system class, will be invoked every fixed update frame.
    /// </summary>
    /// <code>
    /// public class MySystem : EcsSystem, ISystemUpdate {
    ///     public void SystemUpdate() {
    ///         Debug.Log("Every Single Frame");
    ///     }
    /// }
    /// </code>
    [SystemUFrameEvent("Update", "SystemUpdate")]
    public interface ISystemUpdate
    {
        /// <summary>
        /// Called every frame
        /// </summary>
        void SystemUpdate();
    }
    [SystemUFrameEvent("Late Update", "SystemLateUpdate")]
    public interface ISystemLateUpdate
    {
        /// <summary>
        /// Called every frame
        /// </summary>
        void SystemLateUpdate();
    }
    /// <summary>
    /// This interface, when added to a system class, will be invoked every fixed update frame.
    /// </summary>
    /// <code>
    /// public class MySystem : EcsSystem, ISystemFixedUpdate {
    ///     public void SystemFixedUpdate() {
    ///         Debug.Log("Every Single Fixed Frame");
    ///     }
    /// }
    /// </code>
    [SystemUFrameEvent("Fixed Update", "SystemFixedUpdate")]
    public interface ISystemFixedUpdate
    { 
        /// <summary>
        /// Called every fixed frame
        /// </summary>
        void SystemFixedUpdate();
    }
}