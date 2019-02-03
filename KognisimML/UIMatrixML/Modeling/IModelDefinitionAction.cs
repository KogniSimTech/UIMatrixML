namespace UIMatrixML.Modeling
{
    public interface IModelDefinitionAction
    {
        string Action { get; set; }
        string Selector { get; set; }
        object Value { get; set; }
    }
}
