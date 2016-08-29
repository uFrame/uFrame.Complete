namespace uFrame.ECS.Templates
{
    //public partial class ComponentGroupManagerTemplate
    //{
    //    [GenerateMethod]
    //    public virtual bool Filter(_ITEMTYPE_ item)
    //    {

    //        var filterBegin = Ctx.Data.FilterOutputSlot.OutputTo<ActionNode>();

    //        if (filterBegin != null)
    //        {
    //            filterBegin.WriteCode(Ctx);
    //        }

    //        //Ctx.CurrentMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "entityId"));


    //        Ctx._("return true");
    //        return false;
    //    }

    //    [GenerateMethod]
    //    public virtual _ITEMTYPE_ CreateGroup()
    //    {
    //        //var validateInvoke = new CodeMethodInvokeExpression(
    //        //    new CodeThisReferenceExpression(),
    //        //    "Validate");
    //        Ctx.CurrentMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "entityId"));

    //        foreach (var component in Components)
    //        {
    //            Ctx.CurrentMethod.Parameters.Add(new CodeParameterDeclarationExpression(component.Name,
    //                component.Name.ToLower()));

    //            //validateInvoke.Parameters.Add(new CodeVariableReferenceExpression(component.Name.ToLower()));
    //        }
    //        //var condition = new CodeConditionStatement(
    //        //    validateInvoke
    //        //    );

    //        Ctx._("var group = this.gameObject.AddComponent<{0}>()", Ctx.Data.Name);
    //        Ctx._("group.EntityId = entityId");
    //        foreach (var component in Components)
    //        {
    //            Ctx._("group.{0} = {1}", component.Name, component.Name.ToLower());
    //        }
    //        Ctx._("return group");
    //        //Ctx.CurrentStatements.Add(condition);
    //        //Ctx._("return null");
    //        return null;
    //    }

    //    [GenerateMethod, AsOverride]
    //    public void ComponentCreated(IEcsComponent ecsComponent)
    //    {

    //        Ctx._if("Ids.Contains(ecsComponent.EntityId)").TrueStatements._("return");
    //        //Ctx._if("ecsComponent is ComponentGroup").TrueStatements._("return");
    //        foreach (var component in Components)
    //        {
    //            Ctx._("{0} {1}", component.Name, component.Name.ToLower());
    //        }

    //        foreach (var component in Components)
    //        {
    //            Ctx._if("!ComponentSystem.TryGetComponent(ecsComponent.EntityId, out {0})", component.Name.ToLower())
    //                .TrueStatements
    //                ._("return");
    //        }

    //        Ctx._("Ids.Add(ecsComponent.EntityId)");

    //        CodeMethodInvokeExpression invokeAdd = new CodeMethodInvokeExpression(
    //            new CodeThisReferenceExpression(),
    //            "CreateGroup"
    //            );

    //        var groupVar = new CodeVariableDeclarationStatement(Ctx.Data.Name, "group", invokeAdd);

    //        invokeAdd.Parameters.Add(new CodeVariableReferenceExpression("ecsComponent.EntityId"));
    //        invokeAdd.Parameters.AddRange(
    //            Components.Select(_ => new CodeVariableReferenceExpression(_.Name.ToLower()))
    //                .Cast<CodeExpression>().ToArray()
    //            );
    //        Ctx.CurrentStatements.Add(groupVar);

    //        Ctx._("this.OnEvent<ComponentDestroyedEvent>().First(p=> p.Component == {0}).Subscribe(_=>{{ Ids.Remove(group.EntityId); DestroyImmediate(group); }}).DisposeWith(this);",
    //            String.Join("|| p.Component == ", Components.Select(p => p.Name.ToLower()).ToArray())
    //            );
    //        // Put any observables here EXAMPLE:
    //        //damageable.HealthObservable.Subscribe(health =>
    //        //{
    //        //    UpdateGroup(group);
    //        //}).DisposeWith(damageable);


    //    }
    //}
}