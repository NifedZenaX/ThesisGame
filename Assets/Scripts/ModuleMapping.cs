using System.Collections;
using System.Collections.Generic;

public class ModuleMapping
{
    public enum ModuleTypeEnum {
        Shapes_Numbers,
        Wires,
        Forgot_Password,
        Module_Upgrade
    };

    public static Dictionary<ModuleTypeEnum, object> moduleMapping = new Dictionary<ModuleTypeEnum, object>
    {
        { ModuleTypeEnum.Shapes_Numbers, new ShapesNumbersModule()},
        { ModuleTypeEnum.Wires, new WiresModule()},
        { ModuleTypeEnum.Forgot_Password, new ForgotPasswordModule()},
        { ModuleTypeEnum.Module_Upgrade, new ModuleUpgradeModule()},
    };
}
