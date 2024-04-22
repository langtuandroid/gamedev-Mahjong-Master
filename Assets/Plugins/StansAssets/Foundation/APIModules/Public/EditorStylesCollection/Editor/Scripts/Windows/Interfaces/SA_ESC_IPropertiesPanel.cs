namespace SA.Foundation.EditorStylesCollection
{
    
    public interface SA_ESC_IPropertiesPanel
    {
        
        void OnGUI();
        bool CanBeSelected { get; }
    }
}
