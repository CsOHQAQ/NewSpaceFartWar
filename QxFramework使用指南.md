# 萌新的QX开发之旅

## 简介

在QX框架中，游戏被拆分为不同的流程(Procedure)，每个流程中又包含更多执行功能的子模块(submodule)，通过在各个模块中调用QX框架中的方法(包括游戏逻辑，资源调用，ui开关等)，实现推进游戏进行的目的。

QxFramework引导教程的面向对象是有一定Unity基础，但从来没有接触过QX框架的新开发者。因此有许多的范例引导都有可能存在更有效的其他解法，范例只是为了让QX萌新更好理解工具，而不是不能改变的模板（当然，固定的语法是不能改变的）。

## 手把手的QX教程

### 0：从头开始

使用UnityHub，直接打开QxFramework工程，在此框架下进行开发。

由于QxFramework基于Unity2019.4.26f1版本进行构建，属于较老版本的Unity，较于新版本的Unity使用了部分已经过时的冲突插件。如果使用新版本Unity载入QX工程而遇上报错的情况，请尝试删除Assets\Scripts\Tool\EventEditor\UnityPackages\JsonNet目录下的Newtonsoft.Json文件。（如果还是没有解决问题的话，还是联系内部人员吧）

### 1：游戏需要启动(Launcher)

在需要开发的起始场景中，新建一个空物体，挂载Launcher组件（该组件已包含在QX工程中）

注意到Inspector面板中，存在StartProcedure，这里填入游戏所需要执行的第一个流程（开始流程）的名称。

### 2：游戏从流程(ProcedureBase)开始搭建

在QX框架中，游戏本体由一系列流程组成，并通过在不同情况下的流程切换流程以推进游戏进程。

创建一段新代码，该段代码需要引用QxFramework.Core命名空间，并<u>继承命名空间中的ProcedureBase类</u>，这是所有流程的基类。

在ProcedureBase类维护以下常用的公用方法：

可调用方法

> + <u>AddSubmodule(Submodule submodule)</u>：为Procedure添加子模块submodule的方法，会调用子模块的初始化方法。
> + <u>ChangeTo(string ProcedureName)</u>：切换流程的方法，当流程发生转换时，触发当前流程的Leave方法，以及下一个流程的Enter方法。一般由ProcedureManager触发。
> + Init()：游戏启动时被调用一次。
> + Enter()：进入该流程被调用的方法。
> + Update/FixedUpdate()：每帧/固定帧被调用的方法。
> + Leave()：离开该流程时被调用的方法。
> + Destory()：程序销毁时被调用的方法。

可override方法：

> + <u>OnInit()</u>：流程初始化时执行的方法。
> + <u>OnEnter(object args)</u>：被Enter调用，进入流程时执行。（注意看，这个新的参数叫args，他之后才会被讲到）
> + OnUpdate/OnFixedUpdate()：被Update/FixedUpdate()调用，每帧/固定帧执行。
> + <u>OnLeave()</u>：被Leave()调用，离开流程时执行。
> + OnDestory()：被Destory()调用，程序销毁时执行。

我们创建一个名为HelloQXProcedure的流程，将HelloQXProcedure填入Launcher的StartProcedure，便将HelloQXProcedure设置为了开始流程。

在HelloQXProcedure中使用override覆写OnInit方法，添加一个Debug.log，

>```
>public class HelloQXProcedure : ProcedureBase
>{
>    protected override void OnInit()
>    {
>        base.OnInit();
>        Debug.Log("HelloQXProcedure Init");        
>    }
>
>    protected override void OnEnter(object args)
>    {
>        base.OnEnter(args);
>        Debug.Log("HelloQXProcedure Enter");
>    }
>}
>```

开始游戏，测试该流程有没有正常启动呢？

PS：<u>当游戏存在多个流程的时候，游戏开始时会先执行所有流程的Init()方法，而Enter()方法则只会在进入该流程时执行。</u>

### 3：给流程添加子模块(Submodule)

创建一段新代码，该段代码同样需要引用QxFramework.Core命名空间（之后若无特殊标记，则默认引用QxFramework.Core），并继承命名空间中的Submodule类，这是所有子模块的基类。每一个子模块都可以附属新的子模块，并将这些子模块存放在数组中。

在Submodule类维护以下常用公有属性与方法

属性：

> + Submodule ModuleParent：父模块
> + ProcedureBase ProcedureRoot：与此模块关联的流程
> +  List<Submodule> Children：从属于该子模块的子模块数组
> + List<UIBase> openUIs：当前模块打开的UI

可调用方法：

> + SetRootProcedure(ProcedureBase)：设置子模块所属的流程。
> + <u>OpenUI(string, string, object)</u>：调用UIManager打开一个UI（参数分别对应：需要打开的UIPrefab名称，打开后UI的gameobject名称，额外参数）。
> + <u>CloseUI((string uiName, string objName)：,CloseAllUI()</u>：关闭UI相关方法。
> + <u>AddChilren(Submodule sub)</u>：为该子模块添加新的子模块。
> + DestoryChidren(Submodule submodule)：消除该子模块的某个指定的所属子模块。
> + Init()：被ProcedureBase.AddSubmodule调用，该子模块被添加时自动执行的初始化方法。
> + Update/FixedUpdate() ：每帧/固定帧执行的方法。
> + Destroy()：销毁时触发的方法。

可override方法：

> + <u>OnInit()</u> ：被Init()方法调用，子模块被添加时执行。
> + OnUpdate()/OnFixedUpdate()：被Update/FixedUpdate() 调用，每帧/固定帧执行。
> + OnDestroy()：被Destroy()调用，销毁时执行。

我们继续创建一个名为HelloQXSubmodule的流程，返回到HelloQXProcedure中，在OnEnter方法中使用AddSubmodule(new HelloQXSubmodule())，使流程在进入HelloQXProcedure时，自动为其添加一个HelloQXSubmodule子模块。

在添加子模块时，会自动调用Submodule的初始化方法。可以在HelloQXSubmodule中使用override重写OnInit()方法，添加Debug以查看该流程是否正确初始化。（灵活运用debug.log来确定是否执行和执行顺序）

> ```
> public class HelloQXSubmodule : Submodule
> {
>     protected override void OnInit()
>     {
>         base.OnInit();
>         Debug.Log("HelloworldSubmodule Init");
>     }
> }
> ```

~~（这里有BUG，AddChilren不会将子模块正常放入Children列表）~~

（不过本来子模块就用不完，建议是不要子模块套子模块）

### 4：流程管理器(ProcedureManager)跳转流程

虽然游戏流程基类ProcedureBase中存在切换流程的方法，但为了在其他地方仍然可以触发流程跳转，可以使用单例模式的ProcedureManager进行流程跳转。ProcedureManager的开发遵循单例模式，通过ProcedureManager.Instance直接调用方法，而不需要预先持有引用。（单例：Singleton类与MonoSingleton，会讲的）

类ProcedureManager类维护以下常用属性和方法。

属性：

> + ProcedureBase Current: 当前流程
> + ProcedureBase Previous：上一个流程

方法：

> + ChangeTo(string name, object args = null): 跳转到指定流程，额外参数默认为空。
> + ChangeTo<*Procedure*>(object args)：跳转到指定流程（带参数）。其中*Procedure*是一个继承了ProcedureBase的自定义流程，包含对参数项的处理方式。

示例：我们按照之前的方式，创建一个新的Procedure，并在挑选一个的合适位置调用用ChangeTo方法，

> ```
> ProcedureManager.Instance.ChangeTo("NextProcedure")
> ```

其中，NextProcedure是新Procedure的名称，可以利用debug.log观察执行顺序。

#### 4+. 额外参数(object args)的使用方法

在之前讲过的ProcedureBase.OnEnter(object args)方法，与这里提到的ProcedureManager.Instance.ChangeTo<*Procedure*>(object args)方法都提到了object args这一参数。那么这个参数究竟应该如何使用呢？

我们已经知道，调用ChangeTo<*Procedure*>(object args)方法的时候，流程跳转的执行顺序为

> PreviousProcedure.OnLeave()  ===> NextProcedure.OnEnter(object args)

其中，ChangeTo提供的额外参数object args会被提供给下一个流程的OnEnter方法，因此我们可以通过这一特性，在跳转流程是给予额外的指令，下一个进行的流程则会根据额外指令做出相应的调整。

为了更好理解额外参数的使用方式，我们通过一个简单的例子来讲解。

回到我们之前写的跳转流程方法，

> ```
> ProcedureManager.Instance.ChangeTo("Next_Procedure_Name"
> ```

其中，额外参数部分缺省，默认为空值，可以将其修改为

> ```
> ProcedureManager.Instance.ChangeTo("SecondProcedure","To_Next_Procedure");
> ```

在object args对应的位置下，我们输入一个字符串string类型的参数（这里是"To_Next_Procedure"这一字符串），由于在语法中，object类是所有类的基类，所以这里可以填入任何<u>类</u>的额外参数。

接下来进入需要跳转的流程的代码，override覆盖ProcedureBase.OnEnter(object args)方法。由于方法只会知道参数args继承了object类，却并不知道其具体类型，因此我们需要对参数进行<u>强制转换</u>。在这个例子中，我们知道传入的参数一定是一个字符串类型参数，所以首先将这个参数以字符串形式获取下来。

> ```
> string _stringArgs = (string)args;
> ```

此时，_stringArgs便可以当作一个正常的字符串类型参数进行使用。试着使用Debug语句测试是否正确获取到从ChangeTo方法传来的参数（"To_Next_Procedure"）呢？

需要注意的是，由于读取参数的方法使用了强制转换的方法，在使用时请一定要确保每次调用ChangeTo方法进入同一个流程时，传入的参数为同一个规定好的类型，否则会出现空指针报错。

### 5：从标题界面引入UI系统(UIbase与UIManager)

每个游戏，不管是线性叙事还是开放世界探索，是叫人肾上腺素飙升的动作游戏还是吓人一身冷汗的恐怖解密，他们几乎都有一个共同点：都有一个标题界面。

不出意外的话，标题界面常常是玩家进入游戏的第一个流程。~~虽然可能有些不符合游戏开发的逻辑~~，但这并不妨碍我们首先通过标题界面来认识QX框架之中的UI的运行逻辑与相关组件。

简单来说，打开UI的方式是，由UIManager在对应的Canvas下的<u>子物体</u>中，实例化已经完成的UI预制体，实现不同层级的UI显示。

#### A. 创建一个符合格式的Canvas(UIManager)

新建一个Canvas，并在Canvas上挂载UIManager。同时，在该Canvas下创建若干个子物体作为UI的不同渲染层级，子物体的名字没有硬性要求，但由于Hierarchy面板中更靠下的层级会渲染在上层，所以为了便于开发，请将子物体按顺序编号（如Layer0，Layer1……）。

注意，由于UIManager处于单例状态（即整个游戏中只会存在单个UIManager），请不要在同一个场景中挂载多个UIManager。

UIManager类维护以下常用的属性与方法

属性：

> + string FoldPath：UI预制体路径，读取UIprefab时使用。

方法（通常通过Instance执行）：

> + Open(string, int, string, object)：打开UI，参数分别为：UI预制体名，层级，实例化之后UI的GameObject名字，传入的参数。
> + Open(string, UnityEngine.Transform, string, object)：打开UI，参数分别为：UI预制体名，实例化之后Hierarchy面板中的UI名称，输入的参数。
> + OpenUICheck(string)：检测是否开启了某UI，返回bool值。
> + Close(UIBase,string)：关闭传入的UI对象，传入其类型以及GameObject名称，（string通常可省略）。
> + Close(string, string)：匹配UIPrefab的名称与Hierarchy面板中的名称关闭对应UI。
> + Close(string)：匹配UIPrefab名称关闭UI，若存在多个同预制体UI，则优先关闭后实例化的UI。
> + CloseAll()：关闭所有打开的UI。
> + FindUI(string,string)：场景中是否存在某UI，返回bool值。
> + GetUI(string)：根据名字获取场景中UI的脚本，返回相应UIBase。

#### B. 创建一个UI的预制体Prefab(UIBase)

在<u>Canvas</u>下新建子物体，设置尺寸与屏幕匹配。为子物体命名，在其中搭建所需的UI，并在其上挂载一个同名的脚本（理论上是可以不同名的，但这样便于管理，这里将UIPrefab和脚本命名为HelloQXUI）。然后将子物体拖拽到<u>Resources/Prefab/UI</u>目录下，这个目录是读取UI文件的默认目录。可以修改FoldPath，但一定要与存储地址保持一致。（注意，这里的UI不要删除，后续的功能讲解很多会用到此UI）

（注意，在保存UI预制体时，一定要检查UI是否正确加载预览环境，点击进入UIprefab，若其Hierachy面板的顶端存在Canvas(Environment)，则已正确加载，否则有可能出现UI加载不正常的情况。）

UI预制体的脚本需要继承UIBase类才能被正常读取。（不继承倒是可以打开，但没法正常使用相关方法）

UIBase类维护以下常用的属性与方法

属性：

> + Dictionary<string,GameObject> _gos：提供一个快速查找子物体的字典，可以使用子物体的名字快速找到对应的子物体。(注：此方法将在新版本的QX框架中被移除)
> + int UILayer：生成UI的层级，默认层级为2。

可调用方法：

> + Get<T>(string)：快速获取组件，T为泛型类型，需要获取什么就填什么，string为该UI预制体下的某个子物体的名称。
> + CollectObject()：建立包含该UI所有子物体的字典（初始化_gos），以名字字符串为键，对应gameobject为值。
> + DoDisplay(Object args)：当前UI显示时自动执行的方法。
> + DoClose()：当前UI被关闭时自动执行的方法。
> + RegisterDataUpdate<T>(Action<GameDataBase>,string)：为某一个游戏数据注册委托，使得这个游戏数据在更新时能够被当前UI处理。
> + RegisterMessage<T>(T,EventHandler<EventArgs>)：为某一个游戏事件注册委托，使得这个游戏事件出现时能够被UI所处理。

可override方法：

> + OnDisplay(Object args)：被DoDisplay(Object args)调用，UI显示时自动执行。
> + OnClose()：被DoClose()方法调用，UI关闭时自动执行。
> + OnRegisterHandler()：被DoDisplay(Object args)调用，注册消息处理方法时执行。

需要注意的地方是，由于历史原因，UI生成的默认layer为2，即默认生成为UIManager下的第三个子物体（0，1，2），所以初次尝试未能成功生成UI时，不妨检查一下是不是此原因。

做完这些操作，回到HelloQXSubmodule，在OnInit方法中使用OpenUI(string, string, object)方法，虽然看上去需要三个参数，但在较为简单的情况下，后两个参数可以缺省。因此这里可以只用填入UIPrefab的名称用以查找需要实例化的UI。调用方法OpenUI("HelloQXUI")，便可以实例化相应的UI。

#### C.从创建按钮事件初步认识ChildValueBind工具

在HelloQXUI预制体，添加按钮命名为“CloseBtn”（名字随便起，统一就行）作为简单的使用案例。

进入HelloQXUI脚本，写入

> ```
> [ChildValueBind("CloseBtn", nameof(Button.onClick))]
>  Action OnCloseButton;
> ```

其中，ChildValueBind为QX框架中的特性，使用这一句，会首先匹配到名为“CloseBtn”的gameobject，找到其中的Button组件中的OnClick，将接下来的Action与之匹配。（注意：Button需要调用UnityEngine.UI，而Action需要调用System）

接下来override OnDisplay方法，输入语句

> ```
> OnCloseButton = CloseButton;
> ```

由于在定义Action OnCloseButton时，没有给Action指定泛型，因此我们创建一个<u>无参数的void方法</u>CloseButton()，以匹配Action OnCloseButton。

> ```
> private void CloseButton() {}
> ```

若定义Action<string> OnCloseButton，则创建方法

> ```
> private void CloseButton(string obj){}
> ```

依此类推。（不过按钮点击时传入的参数为空，所以这里常用无参数方法）

完成以上操作，脚本在UI启用时，会赋予场景中的“CloseBtn”点击事件，使其在点击的时候自动执行CloseButton()方法。需要注意的是，一个按钮只能绑定一个方法，若重复绑定，则只会执行<u>最后一个绑定的事件</u>，如

> ```
> [ChildValueBind("CloseBtn", nameof(Button.onClick))]
>  Action OnCloseButton;
> [ChildValueBind("CloseBtn", nameof(Button.onClick))]
>  Action OnSubAction;
> ```

OnCloseButton与OnSubAction都绑定了方法，则最后只会执行OnSubAction所绑定的方法；如果语句交换顺序，则只会执行OnCloseButton所绑定的方法。

在应用过程中，我们一般使用此工具来快速为按钮绑定需要执行的方法。当然，为了实现相类似的方法，我们也可以使用Unity中的SetListener方法，但这里不再详细说明。

#### D.UIBase基础方法与_gos字典的应用例

**修正：在新版本的QX框架中，_gos字典被暂停使用，有需要通过名称获取游戏物体请统一使用Get<T>(string)方法**

回到OnDisplay方法，一般的习惯是首先在这里调用一次UIBase的CollectObject()方法，用来初始化_gos字典。~~当然，就算忘了写，框架内部会自动触发，如果你完全相信框架，不写也可以。~~添加这一段可以增强代码的健壮性。

可以在CloseButton()方法中，为之前创建的closeBtn添加不同的功能。首先为方法添加常用的流程跳转功能

> ```
> ProcedureManager.Instance.ChangeTo("SecondProcedure");
> ```

其中，SecondProcedure的创建流程参考前文ProcedureBase。此时点击按钮便会执行跳转流程的操作。

注意：<u>当流程发生跳转时，该流程所打开的所有UI都会自动关闭并被回收入对象池。</u>

以下还有更多应用例。

例一：创建DisappearBtn按钮，OnDisappearButton委托，DisappearButton()方法。在方法中添加

> ```
> _gos["DisappearBtn"].SetActive(false);
> ```

通过按钮在Hierarchy面板中的名称，从_gos字典中快速获取到对应<u>**gameobject**</u>。此时点击按钮，便会执行SetActive(false)，按钮消失。

例二：创建DisableBtn按钮，OnDisableButton委托，DisableButton()方法。在方法中添加

> ```
> Get<Button>("DisableBtn").interactable = false;
> ```

通过UIBase的Get方法快速获取到InteractableBtn按钮的Button组件（也可以是其他任意物体的任意<u>**组件**</u>）。此时点击按钮，便会执行语句，改变该按键的互动状态。

<u>以后我们还会在这里测试更多的方法</u>。

### 6：从游戏管理器(GameMgr)写逻辑方法

一般的游戏开发过程中，都会用到一个游戏管理器(GameManager)用以管理整个游戏的流程运行，其使用之广泛以至于Unity引擎还专门为其添加了一个彩蛋（在Unity中，当你将C#脚本命名为GameManager时，其在Project面板中的图标会变成一个齿轮）。与之对应，QX框架中的GameMgr也是构造游戏核心的重要部分。

与Launcher一样，GameMgr同样需要挂载在一个空物体上。

GameMgr类维护以下常用的属性和方法

属性

> + public List<ModulePair> _modules：存储已经添加的逻辑块，其中ModulePair储存逻辑块(<u>LogicModuleBase</u>)与其对应的枚举类型(<u>ModuleEnum</u>)。（LogicModuleBase与ModuleEnum属于QX框架内部语法）
>

方法

> + public void InitModules()：初始化所有需要的逻辑模块，一般在游戏开始时调用。
> + public static Get<T>()：<u>静态方法</u>。获取一个已经挂载的逻辑块，其中T为泛型，一般填入一个逻辑块接口。注意，这里的Get与UIBase中的Get方法不同，一般通过GameMgr.Get<T>进行调用。

由于其中涉及到QX框架中的通用方法，接下来我们将通过范例一步一步解释相关的类与操作。

#### A. 新建一个管理逻辑块(LogicModuleBase)

新建一个逻辑块脚本，这里我们将其命名为HelloQXManager，该脚本需要继承QX框架中的LogicModuleBase类。LogicModuleBase是所有逻辑块脚本的基类。

LogicModuleBase维护以下常用的方法

可调用方法：

> + protected bool RegisterData<T>(out T data,string key)：注册游戏数据的方法（QXData相关，之后的篇章会讲）
> + protected void SetModify<T>(out T data,string key)：（QXData相关，之后的篇章会讲）

可override方法：

> + Awake()：脚本被实例化时自动执行。
> + Update()/FixedUpdate()/LateUpdate()：每帧执行的方法。
> + OnDestroy()：脚本被销毁时执行。

#### B.为逻辑块创建需要的接口(IxxManager)

在QX框架内，每一个继承LogicModuleBase的管理逻辑块都需要挂载到GameMgr上，统一经过GameMgr调用执行。为了使GameMgr可以正确调用逻辑块中自定义的方法，我们需要为逻辑块添加相应的接口(interface)。这个接口将要包含所有需要的自定义方法的声明。

为了更加直观，我们新建一个接口。注意，接<u>口的命名必须命名为I+逻辑块名(这里需要命名为IHelloQXManager)，否则模块将无法被正常加载</u>。在接口中声明所需要的自定义方法。这里我们声明一个返回值为空的HelloQXFunction()

> ```
> void HelloQXFunction();
> ```

 然后回到HelloQXManager，继承刚刚定义的IHelloQXManager接口。继承接口的类必须要实现接口中的所有方法，我们需要在HelloQXManager中显式实现HelloQXFunction()方法

> ```
> void IHelloQXManager.HelloQXFunction()
> {
> 	Debug.Log("HelloworldFuntion executed");
> }
> ```

此时的HelloQXManager将同时实现基类LogicModuleBase的方法和对应接口中声明的方法。

#### C. 在GameMgr中添加对应的逻辑块枚举(ModuleEnum)

回到GameMgr类中，在代码低端找到枚举类ModuleEnum，向其中添加对应的枚举。注意，枚举类型的名字必须与模块名完全相同（这里命名HelloQXManager）

> ```
> public enum ModuleEnum
> {
>     Unknow = 0,
>     MainDataManager,
>     EventManager,
>     ItemManager,
>     GameTimeManager,
>     HelloQXManager,
>     Max,
> }
> ```

可以看到，ModuleEnum枚举类的初始状态并不是空的，而已经声明了许多其他逻辑块的对应枚举类，这是因为GameMgr是整个项目中通用的游戏管理器，而项目中自带了一部分演示场景，所以已经预设好了一些逻辑块，对逻辑块部分乃至整个QX框架有问题的同志们可以试着去研究这些演示场景作为参考，一定会有所收获。（演示场景保存在QxExamples文件夹中）

接下来返回GameMgr，在Inspector面板中找到“要加载的模块”，向列表中添加已经定义好的枚举类型(这里同样选择HelloQXManager)

完成以上这些步骤之后，代码便可以通过枚举类型的名字匹配到相应的逻辑块，并将其挂载到GameMgr上，从而更加方便的调用该方法。

#### D. 将逻辑块初始化，然后调用一下

由于GameMgr并不会自动进行初始化，因此，通常在游戏开始时调用相关方法，将所有需要加载的逻辑块初始化。

还记得游戏是从什么地方开始的吗？对，游戏开始时，Launcher会启动游戏的第一个流程，进入我们的设置好的StartProcedure(还记得HelloQXProcedure吗？)，在进入这个流程时，我们使用语句

> ```
> GameMgr.Instance.InitModules();
> ```

由于InitModules并不是静态方法，而且GameMgr使用了单例模式，因此可以使用这种方式直接调用GameMgr的初始化方法。接下来就可以开始调用逻辑块的方法了。

还记得我们在UI系统的篇章里所创建的用来测试按钮功能的UIBase吗？我们回到那里，同样也可以调用GameMgr所加载的方法。

回到HelloQXUI，用同样的方法创建“FunctionBtn”按钮和相关的方法，在方法体中调用GameMgr的Get()方法

> ```
> GameMgr.Get<IHellowQXManager>().HelloQXFunction();
> ```

注意，GameMgr中的Get<>()方法与UIBase中的Get方法不同，所以使用时要指定清楚。同时，由于GameMgr.Get方法为静态方法，所以不使用Instance也可以实现跨代码直接调用。

此时启动游戏打开UI，点击新创建的按钮，便可以在UIBase下，通过GameMgr的静态方法直接调用到HellowQXManager逻辑块中自定义的方法HelloQXFunction()。

### 7：用资源管理器(ResourceManager)管理“游戏资源”

游戏资源是一个非常宽泛的概念，一张图片，一段代码，一首背景音乐，只要它保存在Asset之下，就都可以称作广义的游戏资源。在QX框架内，ResourceManager同样遵循单例开发模式，负责统一管理游戏资源包括加载，缓存，实例化等操作。

ResourceManager类维护以下常用的属性和方法

属性

> + private Dictionary<string,Object> _cache：私有属性，用于缓存当前读取到的Object。
> + private Dictionary<string, UnityEngine.Object[]> _arrayCache：私有属性，用于缓存当前读取到的Object数组。
> + public string PersistentDataUrl：持久化的文件的存储地址。

方法

> + public void AddCache(string path,Object obj)：将资源添加到缓存字典中。path为地址，作为键值对的“键”；obj为资源本身，作为键值对的“值”。（不常用）
> + public bool InCache(string path)：判断当前目录下的资源是否在缓存中。path为路径地址。
> + public static bool HasWriteAccessToFolder(string folderPath)：测试对应目录是否有<u>写权限</u>，folderPath为对应文件目录。
> +  public T <u>Load</u><T>(string path, bool instance = false) ：泛型方法，加载对应路径下的文件（添加到_cache字典中），instance为实例化开关，默认为否。
> + public T[] LoadAll<T>(string path)：泛型方法，加载对应路径文件夹下所有的文件。
> + public ResourceRequest LoadAsync<T>(string path)：异步加载资源，一般用于大型项目的进度条加载。（不要随便碰异步，会变得不幸——来自某位前辈）
> +  public GameObject <u>Instantiate</u>(string path, Transform parent = null)：实例化对应路径下的资源，path为对应路径，parent的为指定的父物体，默认为空。实例化通过对象池模式实现。

我们同样通过范例来解释常用方法的具体操作方法。

#### A. 预制体的实例化(Instantiate)

首先，我们在Asset/Resourses/目录下，创建一个名为“HelloQXFolder”的文件夹。

接下来在场景中随便搭建一个场景，无论2d还是3d，人物立绘还是物品建模都可以，但最重要的是：<u>他们都需要包含在一个父物体下</u>，这里我们将这个父物体命名为“HelloQXPrefab”，并在“HelloQXFolder”目录下创建这个物体的预制体。预制体创建完成后就可以从Hierarchy面板中删除，接下来可以通过指令将预制体重新实例化。

还记得之前创建的HelloQXUI吗？我们在合适的地方调用

> ```
> ResourceManager.Instance.Instantiate("HelloQXFolder/HelloQXPrefab");
> ```

由于该方法会自动为地址补全Asset/Resourses/，所以只需要输入在Resourses文件夹内的读取地址即可。同时，由于父物体参数缺省，预制体将没有父物体而直接生成。

当调用该方法时，预先创建的Prefab将会被实例化在Hierarchy面板中。可以通过此方法实例化以prefab形式保存的场景或物体。

#### B. 资源加载(Load)

在流程执行的过程中，先后调用

> ```
> ResourceManager.Instance.Load<GameObject>("HelloQXFolder/HelloQXPrefab");
> ```

> ```
> Debug.Log(ResourceManager.Instance.InCache("HelloQXFolder/HelloQXPrefab"));
> ```

以此观察资源被载入缓存的情况。（请一定要注意调用的先后顺序，顺便复习流程执行的先后顺序）

### 8: 使用消息管理器(MessageManager)跨区域传递消息

通常的游戏开发过程中，常常会用到发布者-订阅者(Publisher-Subscriber)框架。简单来说，发布者与订阅者一般分属于不同的区块，彼此之间不存在直接的引用关系。发布者可以广播不同种类的消息，将其分发给接收对应特定消息的订阅者，订阅者根据接收到的消息做出相应的反应。一个发布者发出的消息可以由不同的订阅者接收，从而通过一段消息在不同的管理区域执行相应的操作；订阅者也可以随时取消对该事件的订阅，暂停接收消息。

在QX框架下，我们通常使用MessageManager来管理该框架。MessageManager遵从单例模式，方法一般使用MessageManager.Instance直接调用。

MessageManager类维护以下常用的方法

> + public MessageQueue<TEventType> <u>Get</u><TEventType>(bool immediate = true) where TEventType : struct ：获取一个消息队列，其中bool immediate = true表示默认接收消息时立即执行(可以修改)，TEventType需要引入一个枚举类。
> +  public void RemoveAbout(object target)：移除与某个对象相关的所有消息队列

其中，消息队列(MessageQueue)是MessageManager中十分重要的一环。

#### A. 先来了解一下消息队列(MessageQueue)

每一个消息队列都会根据一个枚举类(enum)，维护一套消息。在正常开发过程中一般不会直接调用消息队列，而是<u>消息管理器</u>获取到对应的消息队列，然后调用其方法。

MessageQueue类维护以下常用的方法

> + public void Initialize(bool immediate = true)：初始化方法，被MessageManager.Instance.Get方法调用。
> + public void Dispose()：释放消息处理器。
> + public void <u>RegisterHandler</u>(TEventType msgId, EventHandler<EventArgs> callback)：为订阅者注册一个消息的处理方法。其中TEventType对应一个相关的枚举，EventHandler则表示需要与此消息绑定执行的方法。
> +  public void <u>RemoveHandler</u>(TEventType msgId, EventHandler<EventArgs> callback)：为订阅者取消关注，解绑与消息绑定的方法。
> + public void RemoveAbout(object obj)：移除指定消息的处理方法，使该消息的所有订阅者取消订阅此消息。
> + public void RemoveAll()：移除所有消息的处理方法。
> + public void DispatchMessage(TEventType msgId, object sender, EventArgs param = null,bool immediate = true)：广播一条消息，需要定义消息的类型；发布者；EventArgs事件额外参数默认为空(传入参数需要一个继承了EventArgs类的参数类)；默认立即执行(immediate为False时，将会加入消息队列)。

#### B.从枚举类(enum)开始简单搭建消息收发框架

为了建立一条沟通发布者与订阅者的信息渠道，我们首先要保证该渠道内的所有信息属于同一个枚举类，以保证这一条信息渠道不会被其他的消息所干扰。我们将通过示例，从头搭建一条简单的框架。

首先新建一段代码（这里命名为HelloQXMessageType），将其更改为枚举类。（枚举类的基础知识这里不再深入讲解，有需要请自行查阅相关资料）为了使演示更加清晰，先添加First，Second两个枚举。（理论上而言，只要枚举类存在就可以直接使用，放在别的地方也可以，但为了方便演示，这里将枚举类放在了新建代码中）

接下来我们再新建一段代码作为框架中的订阅者（命名为HelloQXSubscriber）。订阅者需要知道自己要响应什么消息，以及响应消息后执行的相应方法。我们先创建方法

>  ```
>  void FirstQXFunction(object sender, EventArgs e)
>  ```

其中，方法的参数分别为<u>消息来源</u>和<u>消息中附带的额外参数</u>，此参数格式为响应消息的方法的固定格式。（方法体使用debug.log方法查看执行情况）

知晓该做什么之后，订阅者还需要知道要响应什么信息，以及什么时候需要开始响应，什么时候需要结束响应，。这里我们使HelloQXSubscriber在Awake时开始响应，使用RegisterHandler方法为其注册

> ```
> MessageManager.Instance.Get<HelloQXMessageType>().RegisterHandler(HelloQXMessageType.First, FirstQXFunction);
> ```

其中HelloQXMessageType是已经创建的枚举类，HelloQXMessageType.First则是订阅者需要响应的消息，而FirstQXFunction则是接收到消息后需要执行的操作。（当然这里也可以先注册方法FirstQXFunction，然后快捷生成，可以自动补齐所需要的方法参数）同样的方法也可以为HelloQXMessageType.Second创建响应方法。

还记得我们之前搭建的UI面板吗？我们依然在那里测试我们的相关功能。

打开HelloQXUI预制体，新建空物体，将HelloQXSubscriber挂载到空物体上。接下来回到HelloQXUI代码中，用教过的方法创建新按钮DispatchBtn（敲黑板，认真复习！），在按钮触发的方法中使用DispatchMessage方法发送消息。

> ```
> MessageManager.Instance.Get<HelloQXMessageType>().DispatchMessage(HelloQXMessageType.First, this);
> ```

其中，HelloQXMessageType.First为发送消息的类别，this代表这条消息由HelloQXUI发送，其他参数缺省。

接下来再开始游戏，看看点击按钮之后，有没有正常触发订阅者中同样注册了消息类别为*First*的相关方法呢？或者试试修改发送消息的类别，看看是否可以出发相对应的其他方法。

需要注意的是，再切换流程等相关操作时，会自动释放掉当前流程的所有订阅关系，因此如果发送消息时没有正常触发，建议检查是否存在不正常的消息处理器释放情况。（开始游戏的时候也会执行一次流程跳转进入开始流程。所以未通过启动流程打开，而直接添加进Hierarchy面板里的订阅关系也会被释放掉，这点需要注意）

#### C. 如何使用EventArgs传入自定义参数

之前我们讲到，同一个渠道的消息可以归到同一个枚举类下，但是如果需要消息需要更高的自定义程度，就不能只通过枚举来区分不同的消息。因此，我们需要通过为消息添加自定义参数来达到用同一条消息的不同参数执行不同的效果。EventArgs的用法和之前讲到的额外参数(object args)的用法非常类似，这里同样通过简单的例子进行讲解。

首先，我们新建一个类命名为HelloQXEventArgs，注意这个类需要继承System.EventArgs基类。我们可以为以此格式创建的类添加各类属性，这里我们添加

> ```
> public string name;
> 
> public string description;
> ```

并为其准备一个简单的构造方法

>     ```
>     public HelloQXEventArgs(string _name,string _description)
>     {
>         this.name = _name;
>         this.description = _description;
>     }
>     ```

接下来回到发送消息的指令，为其添加新建的参数

> ```
> MessageManager.Instance.Get<HelloQXMessageType>().DispatchMessage(HelloQXMessageType.First, this, new HelloQXEventArgs("QX", "The Best Development Framework"));
> ```

这样，发送出去的消息就附带了构建HelloQXEventArgs的信息（这里是两个字符串的信息）。~~订阅者当然可以选择忽略此信息，不过这还有什么发信息的必要吗？~~订阅者可以获取到此信息，并对行为做出相应的调整。

回到订阅者位置，还记得之前的方法的参数中有附带额外参数这一个伏笔吗？这个参数为方法传入了额外的信息。不过，由于参数默认的类型为EventArgs，而传入的参数类型为HelloQXEventArgs，需要做一些处理才能正常读取到添加的属性

> ```
> ((HelloQXEventArgs)e).name
> ```

使用强制转换，便可以获取到新创建的参数类中的属性。

需要注意的是，由于处理中使用了强制转换，在这样构筑代码的过程中请一定要保证此订阅者只会接受到该类型的参数，否则会出现空指针报错。

### 9：使用关卡管理器(LevelManager)控制场景跳转

一般情况下，一个游戏通常会存在许多关卡，但同一时间内只会存在一个关卡。为了使关卡的加载和卸载更方便管理，我们可以将不同的关卡保存在不同的场景(Scene)中，然后通过LevelManager直接管理场景。

LevelManager类维护以下常用方法：

可调用方法

> + void OpenLevel(string levelName, Action<string> onCompleted)：根据场景名称打开一个场景，将其设置为当前关卡，并触发一个以关卡名为参数的方法，在场景打开后执行。
> + void CloseLevel(Action<string> onClose = null)：关闭管理器保存的当前关卡，并触发一个以关卡名为参数的方法（默认为空）。

LevelManager是一个非常简单好用的场景控制器，接下来通过例子讲解具体流程。

#### A.创建关卡场景(Scene)

新建两个场景，分别命名为“QXLevel_1”，“QXLevel_2”，场景中随意添加一些元素，作为我们的关卡。场景存储的位置没有要求，但是请一定要在File-BuildSettings中将所有需要通过LevelManager管理的场景build进去，否则无法正常进行场景加载。

#### B.调用相关方法管理关卡场景

我们这里同样通过HelloQXUI的按钮执行相关的方法。我们首先创建一个参数为string字符串类型的方法

>     ```
>     private void OnChangeToLevel(string obj)
>     {
>        	Debug.Log(obj);
>     }
>     ```

然后继续为按钮绑定打开Level的方法

>     ```
>     private void FirstLevelButton()
>     {
>         LevelManager.Instance.OpenLevel("Level_1", OnChangeToLevel);
>     }
>     ```

此时打开游戏点击对应按钮，可以看到，关卡场景被添加到了Hierarchy面板中，与最初的场景并列。这是因为加载场景时，LevelManager会使用LoadSceneMode.Additive模式加载场景（具体逻辑可以自行查找资料）。同时，从Debug.Log(obj)语句的输出可以看出，传入该方法的参数为所需要加载场景的名字。

同样的道理，在其他按钮中调用

> ```
> LevelManager.Instance.CloseLevel(OnChangeToLevel);
> ```

则会自动关闭保存在LevelManager中的当前关卡场景。

需要注意的是，由于打开关卡OpenLevel语句并不会自动关闭前一个场景，因此若多次连续执行了OpenLevel方法后再调用关闭关卡CloseLevel，则只会成功关闭最后一个打开的关卡，而不能关闭之前已经打开的关卡。所以请一定要确保每次打开新的关卡时关闭之前的关卡。（这里需要进行优化）

### 10：QX框架下的游戏数据管理(QXData)

游戏中有大量的数据，包括血量法力值，装备持有状况，关卡完成情况等等。这些数据通常需要在玩家退出了游戏之后仍然被保存，直到再度打开游戏后重新读取，以还原上一次游戏结束时的状况。QXData类为玩家提供了这样一个管理数据的好工具。

QXData类维护以下常用的属性和方法：

属性

> + public TableAgent TableAgent：TableAgent表格管理类，负责读取csv文件的数据，详见TableAgent章节。
> + public float TimeSize：游戏时间进行的快慢，默认为1。

可调用方法

> + public T Get<T>(string key = "Default") where T : GameDataBase, new()：获取保存在GameDataBase类中的游戏数据。
> + public bool InitData<T>(out T data, string key = "Default") where T : GameDataBase, new()：初始化游戏数据，注册需要被保存的数据，被**<u>LogicModuleBase.RegisterData</u>**方法调用
> + public void SetModify<T>(T t, object modifier, string key = "Default") where T : GameDataBase, new()：被<u>**LogicModuleBase.SetModify**</u>方法调用。
> + public void SaveToFile(string FileName)：保存所有存储的数据到指定路径，参数为保存文件的名称。
> + public bool LoadFromFile(string FileName)：读取固定路径下指定名称的存档文件。
> + public void DeleteFile(string FileName)：删除指定路径与名称的存档文件。
> + public void SetTableAgent()：为表格管理器读取固定文件夹下所有的CSV文件。

在一般的应用场景中，我们通常使用QXData类来制作存档功能，接下来我们通过一个简单的例子讲解存档的全过程。

#### A. 用游戏数据类(GameDataBase)整合需要保存的数据

为了使游戏数据可以被正常保存，我们需要制作一个专门用于保存的数据类，这个类既需要能被QXData正常读取，同时也要整合所有需要被保存的数据。我们创建HelloQXGameData数据类，让它继承GameDataBase基类，并为其添加需要保存的数据。

> ```
> public class HelloQXGameData : GameDataBase
> {
>     public int QXID;
>     public string QXString;
>     public bool QXBool; 
> }
> ```

创建好数据类后，我们需要在一个特定的时机注册该数据。通俗来说，就是告诉QXData数据管理器，HelloQXGameData保存的数据是需要被保存的。这里我们通过逻辑管理块(LogicModuleBase)来实现这一过程（顺便复习一下之前的知识点）。

#### B.注册需要被保存的数据(RegisterData)

按照之前讲过的方法，创建HelloQXDataManager逻辑块和IHelloQXDataManager接口。

在逻辑块中创建一个数据类用于管理需要保存的数据。

>         ```
>         private HelloQXGameData _helloQXData;
>         public HelloQXGameData HelloQXGameData
>         {
>             get
>             {
>                 _helloQXData = QXData.Instance.Get<HelloQXGameData>();
>                 return _helloQXData;
>             }        
>         }
>         ```

因为每次读取时都会重新生成一份数据，之前获取到的数据可能是过时的，获取存档数据的时候请务必从QXData里直接获取。可以把以上定义数据类的写法视为固定方法。

接下来override逻辑块基类的Init()，使用以下语句

> ```
> public override void Init()
> {
>     base.Init();
>     if (!RegisterData(out _helloQXData))
>     {
>         HelloQXGameData.QXString = "";
>         HelloQXGameData.QXID = 0;
>         HelloQXGameData.QXBool = false;
>     }
> }
> ```

其中， **<u>if (!RegisterData(out _helloQXData))</u>**是一句非常通用的数据注册方法。其运作原理大致可以解释为：在初始化时，由于HelloQXGameData游戏数据类并没有被注册过，所以RegisterData方法会返回False值，因此这句话不仅会自动注册数据类，还会执行if语句下的方法(注意!符号)为数据初始化。（当然以上定义严格来说可能不太准确，但暂时可以这样理解）

建议：可以将以下公式

> ```
> if (!RegisterData(out <GameDataBase>))
> 
> {
> 
> 	/*数据初始化方法*/
> 
> }
> ```
>

作为注册数据的固定方法。

#### C.添加各种方法进行测试

##### 1.保存方法

我们在接口和逻辑块中创建方法，然后还是使用HelloQXUI中的按钮和其他组件执行（相关的操作都还记得吗？）。首先创建保存方法

>         ```
>         public void Save()
>         {
>             QXData.Instance.SaveToFile("QXDataSave");
>         }
>         ```

SaveToFile方法会讲当前数据保存到一个特定文件夹下，并将文件命名为QXDataSave至于这个地址究竟在哪里嘛。。。开始吟唱：

> C:\Users\用户名\AppData\LocalLow\CompanyName\ProductName

至于CompanyName和ProductName，可以在Unity面板Edit-ProjectSettings-Player面板中查询到。

使用按钮从GameMgr调用该逻辑块方法(什么？你忘了？去6-D章节看看呢？)，接下来去路径下查看，应该就能找到QXDataSave文件。

##### 2.读取方法

游戏项目中保存和读取通常是不可分割的，接下来我们创建读取方法。为了使过程更加直观，我们将会再绑定一个Text组件用于展示读取到的文件。我们首先创建方法

>          ```
>          public string DisplayStr()
>          {
>             string value = HelloQXGameData.QXString + HelloQXGameData.QXID + HelloQXGameData.QXBool;
>             return value;
>          }
>          ```

用来将获取到的数据类中的信息转化为一个字符串。

接下来创建读取方法

>         ```
>         public bool Load()
>         {
>             return QXData.Instance.LoadFromFile("QXDataSave");
>         }
>         ```

LoadFromFile方法可以从存档的默认保存路径读取一个指定名称的文件，如果读取不到该文件就会返回False，读取到则返回true。所以在进行保存读取操作时，一定要遵循命名规范。

接下来回到UI代码段中，为按钮注册方法

>         ```
>         private void LoadButton()
>         {        
>             if(GameMgr.Get<IHelloQXDataManager>().Load())
>             {
>                 Get<Text>("DisplayText").text = GameMgr.Get<IHelloQXDataManager>().DisplayStr();
>             }
>             else
>             {
>                 Get<Text>("DisplayText").text = "无存档";
>             }
>         }
>         ```

若成功读取到存档文件，则会在相应的Text组件中显示DisplayStr方法读取到的信息，若并没有找到，则会显示“无存档”并抛出错误。

此时可以手动删除路径下已经存在的存档文件，观察在存档存在与不存在时，方法执行情况的区别。

##### 3.更改数据

通常情况下，我们需要将游戏中更新的值传入到所需要保存的数据类中。这里我们使用输入框(InputField)组件作为输入新数据的方法。首先创建更改数据的方法

>         ```
>         public void ChangeData(string value)
>         {
>             HelloQXGameData.QXString = value;
>         }
>         ```

ChangeData方法可以将读取到的字符串数据赋值给需要保存的数据类中的QXString属性，接下来我们从输入框中读取字符串

>         ```
>         private void ChangeButton()
>         {
>             GameMgr.Get<IHelloQXDataManager>().ChangeData(Get<InputField>("Input").text);
>         }
>         ```

进入游戏后在输入框中输入信息，通过按钮调用ChangeButton()方法，便将更新的信息传入了数据类，但此时这一信息并没有被保存，需要再次调用保存方法才能将此信息保存入存档文件。此时再调用读取方法，便可以获取到新输入的字符串信息。

##### 4.删除存档

某些情况下，我们会需要删除指定存档，可以创建方法

>         ```
>         public void DeleteSave()
>         {        
>             QXData.Instance.DeleteFile("QXDataSave");
>         }
>         ```

接下来通过相应的方式调用此方法，便可以删除指定文件夹下名称为QXDataSave的存档文件。

通过以上四个基础方法不同顺序的组合产生的结果，就已经足够理解整个存档系统的基础操作方法，现在放手去做游戏的存档系统吧。

### 11：通过TableAgent管理用表格存储的数据

表格管理器(TableAgent)是QX框架下负责处理CSV逗号分隔文件的实用工具，由于表格数据也是游戏数据的一种，属于数据管理的范畴，因此通常情况下由QXData管理器进行调用。QXData类持有一个TableAgent类的对象，通过QXData.Instance.TableAgent调用相关方法。

TableAgent类维护以下<u>常用</u>的属性与方法：

属性

> + private Dictionary<string, Dictionary<TableKey, string>> _tableAgent：保存所有"表格"的名称与其所有内容的键值对。Dictionary<TableKey, string>字典保存每一个内容的在表中的相对位置(TableKey)和位置中的内容
> + private Dictionary<string, List<string>> _keys：保存表格名称与表格的所有纵坐标值列表的键值对。
> + private Dictionary<string, List<string>> _key2s：保存表格名称与表格的所有横坐标值列表的键值对。

常用方法

> + public string GetString(string name, string key1, string key2)：根据表格名称，纵坐标值和横坐标值查找表格中的某项元素，返回字符串类型。
> + public float GetFloat(string name, string key1, string key2)：根据表格名称，纵坐标值和横坐标值查找表格中的某项元素，返回浮点数类型。（若不能转换成浮点数则会抛出错误）
> + public int GetInt(string name, string key1, string key2)：根据表格名称，纵坐标值和横坐标值查找表格中的某项元素，返回整型类型。（若不能转换成整型数则会抛出错误）
> +  public string[] GetStrings(string name, string key1, string key2)：根据表格名称，纵坐标值和横坐标值查找表格中的某项元素，返回一个由默认分隔符分隔开的字符串数组。(如读取到的元素为“1|2|3”，则数组读取到的元素分别为“1”，“2”，“3”)
> + public int GetDicCount(string name)：根据表格名称，查找当前表格的纵向长度。
> + public List<string> CollectKey1(string name)：根据表格名称，返回一个包含所有纵坐标(字符串类型)的列表。
> + public List<string> CollectKey2(string name)：根据表格名称，返回一个包含所有横坐标(字符串类型)的列表。

QXData类中的SetTableAgent()方法会将固定文件夹下的所有CSV文件读取到_tableAgent属性中，所以如果要使用TableAgent，一般会在游戏开始时调用该方法。

#### A.从TableItemKey结构体，谈谈如何读取CSV内容

如果我们使用Excle打开一个CSV文件，会发现它与普通的表格十分相似，只是普通的表格可以包含多个工作表(Sheet)，而CSV只能包含一个。而我们用记事本打开，会发现它是一个将表格中的各个元素用逗号分隔开的文本文件（所以在配置CSV文件的时候，请一定要慎用逗号，最好不用），其结构类似于一个二维的字符串数列。

而TableItemKey结构体

> ```
> public struct TableItemKey
> {
>     public string TableName;
>     public string Key1;
>  public string Key2;
>     public TableItemKey(string table, string key1, string key2)
>  {
>         TableName = table;
>         Key1 = key1;
>         Key2 = key2;
>     }
>    }
>    ```

保存这个二维数组的纵坐标(Key1)和横坐标(Key2)，与坐标系中的横纵坐标不同的是，Key1与Key2其实都是数组中的元素。

以表格举例

| TableName | key2_1   | key2_2   |
| --------- | -------- | -------- |
| key1_1    | Item_1_1 | Item_1_2 |
| key1_2    | Item_2_1 | Item_2_2 |

其中Key1和Key2都是可以自定义的，并不是固定的数值。

同时，由于一个CSV文件其实可以包含多个这种形式的表格，<u>因此表格的名称并不是文件本身的名称，而是最左上角的元素</u>。

而一个CSV文件包含多个表格的格式是

| TableName_1 | 1_key2_1   | 1_key2_2   |
| ----------- | ---------- | ---------- |
| 1_key1_1    | 1_Item_1_1 | 1_Item_1_2 |
| 1_key1_2    | 1_Item_2_1 | 1_Item_2_2 |
| ###         |            |            |
| TableName_2 | 2_key2_1   | 2_key2_2   |
| 2_key1_1    | 2_Item_1_1 | 2_Item_1_2 |
| 2_key1_2    | 2_Item_2_1 | 2_Item_2_2 |

使用"###"字符串对不同表格进行分割，读取CSV文件(SetTableAgent)时，将会自动获取到所有表格的TableName，Key1，Key2，元素等信息，存储到对应的键值对中。

#### B.配置CSV文件

为了使CSV文件具有更好的可视性，我们通常采用Excle编辑处理而不是头铁用文本处理方式。使用表格的形式能够让我们更好的理解CSV的纵横关系。我们首先新建一个普通的xlsx表格文件，删除其中多余的工作表，并将表格另存为CSX UTF-8(逗号分隔)格式（UTF-8是为了适配表格中的中文内容）。接下来将保存好的CSV文件放入与Asset平级的Table文件夹，放在这里的CSV文件通常被称为“外表”，由于与Asset平级，其并不在Unity的Project面板中显示，需要从单独打开。我们以下列表格为CSV范例，新建CSV文件“QX表格”，内容为：

| HelloQXTable | First   | Second  |
| ------------ | ------- | ------- |
| 1            | HelloQX | 1       |
| 2            | 3.14    | A\|B\|C |

再次强调，若需要使用TableAgent相关的方法，则需要在游戏开启的时机调用QXData.Instance.SetTableAgent()方法，这里我们选择在ProcedureBase.Init时调用该方法。

> ```
> protected override void OnInit()
> {
>    	QXData.Instance.SetTableAgent();     
> }
> ```

回到Unity面板，在菜单栏中找到GameTool/Build CSV File，对文件进行初始化，此时，QX框架会自动解析外表，在Assets\Resources\Text\Table目录下生成名为HelloQXTable的CSV文件（文件名由外表表头决定，而非外表名称决定）。相应的，新生成的表格一般被称为“内表”。生成内表之后，表格便可以被正常读取与使用。若外表中使用了"###"字符串分割表格，则会生成多份内表。

#### C.搭建相关的使用场景

这里只示范最基础的获取表格字符串内容的范例，其他情况可以在此框架中自行尝试。在HelloQXUI中创建：按钮GetContentBtn；输入框InputKey1，InputKey2；文本TableText

为按钮绑定方法

> ```
> private void GetContentButton()
>     {
>         string key1 = Get<InputField>("InputKey1").text;
>         string key2 = Get<InputField>("InputKey2").text;       
>         string content = QXData.Instance.TableAgent.GetString("HelloQXTable", key1, key2);
>         Get<Text>("TableText").text = content;
>     }
> ```

该方法会从两个输入框中分别读取Key1和Key2，并将从表格中获取到的值传给content并显示在文本中。当未能匹配到对应的值时，content会被设定为空值，但并不会报错。输入正确的坐标即可获取到相对应的字符串内容。

如果有需要获取某一个Key1或Key2的值，则可以使用CollectKey1()方法获取列表，然后再根据索引获取内容。
