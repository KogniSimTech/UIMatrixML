namespace UIMatrixML.Modeling
{
    public class ModelDefinitionAction : IModelDefinitionAction
    {
        public string Action { get; set; }
        public string Selector { get; set; }
        public object Value { get; set; }

        public ModelDefinitionAction()
        {
            this.Action = null;
            this.Selector = null;
            this.Value = null;
        }
    }
}
