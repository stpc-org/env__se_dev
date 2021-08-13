/***************************************************************************************************
*
* ### In-Game Script Development Template (脚本英文全称) ###
* ### IGSDT(脚本英文缩写) | 这里填写脚本中文名 --------- ###
* ### Version 0.0.0(脚本版本号) | by XXXXXXX(你的名字) - ###
* ### STPC旗下SCP脚本工作室开发 欢迎加入STPC ----------- ###
* ### STPC主群群号:320461590 我们欢迎新朋友 ------------ ###
* 
* STPC组织仓库网址:
* https://github.com/stpc-org
* (这里可以放脚本的说明书网址)
* 
***************************************************************************************************/

//在游戏内使用脚本时确保下面一行代码被注释
//在开发过程中取消注释来获取完整的代码提示和代码补全
#define DEVELOP

#if DEVELOP
//用于IDE开发的 using 声明
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRage.Game;
using VRageMath;

//TODO: 在这里设置一个命名空间名称, 名称随意但保证不同脚本的命名空间不同
//我推荐使用全大写的下划线命名方式
namespace TEMPLATE
{
	class Program:MyGridProgram
	{
#endif

		#region 脚本字段

		//字符串 脚本版本号
		readonly string str__script_version = "TEMPLATE V0.0.0";
		//数组 运行时字符显示
		string[] array__runtime_chars = new string[]
		{
	"",
	"",
	"R",
	"RU",
	"RUN",
	"RUNN",
	"RUNNI",
	"RUNNIN",
	"RUNNING",
	"RUNNING",
		};


		/*
		 * TODO: 在这里设置脚本的字段, 这些字段的生命周期与脚本一致
		 * 建议在此存储脚本选项和存储一些脚本运行时所必要的对象
		 * 本模板在下面默认地提供了一个用于设置脚本更新频率的变量
		 * 这个 period__update_output 变量请勿删去, 有一些代码依赖此值
		 */

		//周期 更新输出(默认每秒3次)
		int period__update_output = 20;

		//以上是脚本配置字段

		//列表 显示器
		List<IMyTextPanel> list_displayers = new List<IMyTextPanel>();
		//列表 显示器提供者 
		List<IMyTextSurfaceProvider> list__displayers_providers = new List<IMyTextSurfaceProvider>();
		//列表 显示单元
		List<DisplayUnit> list__display_units = new List<DisplayUnit>();

		//枚举变量 脚本运行模式(默认常规模式)
		ScriptMode mode_script = ScriptMode.Regular;

		//计数 运行命令数
		long count__run_cmd = 0;
		//计数 更新
		long count_update = 0;
		//索引 当前字符图案
		long index__crt_char_pattern = 0;
		//次数 距离下一次 字符图案更新
		long times__before_next_char_pattern_update = 0;
		//次数 距离下一次 更新输出
		long times__before_next_output_update = 0;

		//字符串构建器 默认信息 脚本默认显示的信息
		StringBuilder string_builder__default_info = new StringBuilder();
		//字符串构建器 测试信息 脚本测试时用于输出
		StringBuilder string_builder__test_info = new StringBuilder();
		//脚本配置
		CustomDataConfig config_script;
		//配置集合 脚本
		CustomDataConfigSet config_set__script;
		/*
		 * TODO: 这里可以添加其它的配置集合, CustomDataConfig 对象支持动态配置,
		 * 也就是说可以先解析CD数据, 之后根据已有的数据决定是否添加下一个配置集
		 * 以实现动态效果
		 */

		#endregion

		#region 脚本入口

		/***************************************************************************************************
		* 构造函数 Program()
		***************************************************************************************************/
		public Program()
		{
			Echo("<execute> init");

			//初始化脚本
			init_script();

			Echo("<done> init");
		}

		/***************************************************************************************************
		* 入口函数 Main()
		***************************************************************************************************/
		void Main(string str_arg,UpdateType type_update)
		{
			switch(type_update)
			{
				case UpdateType.Terminal:
				case UpdateType.Trigger:
				case UpdateType.Script:
				{
					run_command(str_arg);
				}
				break;
				case UpdateType.Update1:
				case UpdateType.Update10:
				case UpdateType.Update100:
				{
					update_script();
				}
				break;
			}
		}

		#endregion

		#region 成员函数

		//脚本更新 脚本每一次更新都会执行该函数, 在该函数内编写你的核心逻辑
		//当然我不是指把所有的代码都写在这一个函数里, 这个函数可以调用其它一些自定义函数
		void update_script()
		{
			if(mode_script==ScriptMode.Regular)
			{
				//TODO: 这里可以检查脚本的运行模式, 执行你设计的逻辑, 没有其它运行模式可以删去这段代码
			}

			//TODO: 这里写脚本每一次(自动)更新时需要执行的逻辑



			//信息显示
			display_info();
			//更新次数+1
			++count_update;
		}

		//执行指令
		void run_command(string str_arg)
		{
			var cmd = split_string(str_arg);//空格拆分
			++count__run_cmd;//更新计数
			if(cmd.Count==1)//判断 cmd.Count 即可知道这个命令的参数个数, 如果是1就代表没有参数
			{
				switch(cmd[0])//检查命令
				{
					case "cmd_0"://命令对应的字符串
					{
						//TODO: 这里写命令对应的代码
					}
					break;
					case "cmd_1"://开火
					{
						//TODO: 这里写命令对应的代码
					}
					break;
					//TODO: 下面可以添加你设计的命令
				}
			}
			else
			{
				//下面的命令是带有一个整数参数的, 需要多个参数也以此类推
				//TODO: 如果没有带参数的命令可以删除
				int index_group = 0;
				int.TryParse(cmd[1],out index_group);
				switch(cmd[0])//检查命令
				{
					case "cmd_2":
					{
						//TODO: 这里写命令对应的代码
					}
					break;
					//TODO: 下面可以添加你设计的命令
				}
			}
		}

		//输出信息 输出信息到编程块终端和LCD
		void display_info()
		{
			if(times__before_next_output_update!=0)
			{
				--times__before_next_output_update;
				return;
			}
			else
				times__before_next_output_update=period__update_output;
			--times__before_next_output_update;

			//清空
			string_builder__default_info.Clear();
			//下面这段代码可以为你的脚本设置信息输出(输出到编程块终端)
			string_builder__default_info.Append(
				"<script> "+str__script_version+array__runtime_chars[index__crt_char_pattern]
				+"\n<mode_script>"+mode_script
				+"\n<count_update> "+count_update
				+"\n<count_trigger> "+count__run_cmd
				//TODO: 在这里添加其它的信息显示, 自行设计
				);
			Echo(string_builder__default_info.ToString());

			//更新动态字符图案
			if(times__before_next_char_pattern_update==0)
			{
				times__before_next_char_pattern_update=1;
				++index__crt_char_pattern;
				if(index__crt_char_pattern>=array__runtime_chars.Length)
					index__crt_char_pattern=0;
			}
			--times__before_next_char_pattern_update;

			//TODO: 这里可以写一些别的信息显示的代码

			//这里遍历显示单元进行输出
			foreach(var item in list__display_units)
			{
				if(item.flag_graphic)
				{
					//图形化显示

					if(item.displayer.ContentType!=ContentType.SCRIPT)
						continue;

					switch(item.mode_display)
					{
						case DisplayUnit.DisplayMode.General:
						//模板默认显示常规数据时不能使用图形化显示, 因此这里绘制一个错误图案
						draw_illegal_lcd_custom_data_hint(item.displayer);
						break;

						//TODO: 在这里编写脚本如何根据不同的显示模式为LCD显示信息

					}
				}
				else
				{
					//非图形化显示

					if(item.displayer.ContentType!=ContentType.TEXT_AND_IMAGE)
						continue;

					switch(item.mode_display)
					{
						case DisplayUnit.DisplayMode.General:
						item.displayer.WriteText(string_builder__default_info);
						break;

						//TODO: 在这里编写脚本如何根据不同的显示模式为LCD显示信息

						case DisplayUnit.DisplayMode.None:
						item.displayer.WriteText("<warning> illegal custom data in this LCD\n<by> script TEMPLATE");
						break;
					}
				}
			}

			//显示测试信息
			/*
			 * TODO: 如果你的脚本还处在测试阶段, 可以为添加信息输出到 string_builder__test_info 对象中
			 * 然后取消下面这行代码的注释, 如果你的脚本已经无需测试了, 请把下面这行代码注释掉
			 */
			//Echo(string_builder__test_info.ToString());
		}

		//绘制 提示 LCD的自定义数据非法
		void draw_illegal_lcd_custom_data_hint(IMyTextSurface surface)
		{
			//帧缓冲区
			MySpriteDrawFrame frame = surface.DrawFrame();
			//获取显示器大小
			Vector2 size = surface.SurfaceSize;

			//绘图元素 红色圆环
			MySprite element__red_annulus = new MySprite()
			{
				Type=SpriteType.TEXTURE,
				Data="CircleHollow",
				Position=size*(new Vector2(0.5f,0.4f)),
				Size=size*0.6f,
				Color=new Color(255,0,0),
				Alignment=TextAlignment.CENTER,
			};
			frame.Add(element__red_annulus);

			//绘图元素 红色叉
			MySprite element__red_cross = new MySprite()
			{
				Type=SpriteType.TEXTURE,
				Data="Cross",
				Position=size*(new Vector2(0.5f,0.4f)),
				Size=size*0.4f,
				Color=new Color(255,0,0),
				Alignment=TextAlignment.CENTER,
			};
			frame.Add(element__red_cross);

			//白色文字
			MySprite element__text = new MySprite()
			{
				Type=SpriteType.TEXT,
				Data="<warning> illegal custom data in this LCD\n<by> script AMCCS",
				Position=size*(new Vector2(0.5f,0.8f)),
				Color=Color.White,
				Alignment=TextAlignment.CENTER,
				FontId="White",
				RotationOrScale=1.0f,
			};
			frame.Add(element__text);

			//刷新缓冲区
			frame.Dispose();
		}


		//初始化脚本
		void init_script()
		{
			init_script_config();//初始化脚本配置
			string str_error = check_config();
			//检查配置合法性
			if(str_error!=null) Echo(str_error);

			//初始化配置
			config_script.init_config();
			//显示错误信息
			Echo(config_script.string_builder__error_info.ToString());

			//TODO: 在这里编写你自定义的脚本初始化逻辑, 比如说获取方块对象啊等等

			//初始化显示单元
			init_script_display_units();

			//检查是否出现错误
			if(str_error==null&&!config_script.flag__config_error)
				//TODO: 若没有错误则设置执行频率, 选择你的脚本自动执行频率, 如果你的脚本不需要自动执行, 可以删除这段代码
				Runtime.UpdateFrequency=UpdateFrequency.Update1;
		}

		//检查脚本配置(配置合法返回null, 否则返回错误消息)
		string check_config()
		{
			string info = null;

			/*
			* TODO: 在这里检查脚本的选项是否合法 (比如说数值参数是否太大或者太小等等)
			* 如果发现错误的配置请返回一个string用来描述错误的具体情况, 这将反馈给用户并引导用户修正
			*/

			//如果没有错误就返回null
			return null;
		}

		//拆分字符串
		List<string> split_string(string str)
		{
			List<string> list = new List<string>(str.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries));
			return list;
		}

		List<string> split_string_2(string str)
		{
			List<string> list = new List<string>(str.Split(new string[] { "\n" },StringSplitOptions.RemoveEmptyEntries));
			return list;
		}

		//初始化脚本显示单元
		void init_script_display_units()
		{
			//获取LCD编组
			//TODO: 按你的开发需求来获取用于脚本信息输出的方块
			IMyBlockGroup group_temp = GridTerminalSystem.GetBlockGroupWithName("");

			if(group_temp!=null)
			{
				//获取编组中的显示器和显示器提供者
				group_temp.GetBlocksOfType<IMyTextPanel>(list_displayers);
				group_temp.GetBlocksOfType<IMyTextSurfaceProvider>(list__displayers_providers);
			}

			Dictionary<IMyTextSurface,List<string>> dict = new Dictionary<IMyTextSurface,List<string>>();

			foreach(var item in list_displayers)
				dict[item as IMyTextSurface]=split_string(item.CustomData);

			foreach(var item in list__displayers_providers)
			{
				//以换行拆分
				var list_lines = split_string_2((item as IMyTerminalBlock).CustomData);

				foreach(var line in list_lines)
				{
					//拆分行
					var list_str = split_string(line);
					int index = 0;

					if((!int.TryParse(list_str[0],out index))||index>=item.SurfaceCount||index<0)
						continue;//解析失败或者索引越界, 跳过
					list_str.RemoveAt(0);
					dict[item.GetSurface(index)]=list_str;
				}
			}

			//遍历显示器
			foreach(var pair in dict)
			{
				//拆分显示器的用户数据
				List<string> list_str = pair.Value;
				bool flag_illegal = false;
				int offset = 0;

				DisplayUnit unit = new DisplayUnit(pair.Key);

				if(list_str.Count==0)
					unit.mode_display=DisplayUnit.DisplayMode.General;//用户数据为空
				else
				{
					//以 "graphic" 结尾
					if(list_str[list_str.Count-1].Equals("graphic"))
					{
						offset=1;
						unit.flag_graphic=true;
					}

					//用户数据不为空
					switch(list_str[0])
					{
						case "graphic":
						case "general":
						unit.mode_display=DisplayUnit.DisplayMode.General;
						break;
						case "idntifier"://脚本控制如何输出到LCD的方式是根据其CD中的对应配置
						{
							/*
							* TODO: 这里通过解析LCD中的CD数据来设置 DisplayUnit类型的对象 "unit"的数据
							* 之后在脚本的主循环 (update_script()函数) 中调用的 display_info() 函数
							* 就可以根据 DisplayUnit 实例的数据来显示不同的信息
							*/
							if(true)
							{
								//TODO: 在这里写你的解析逻辑
							}
							else
								//如果发现CD的数据并不合法, 就把这个bool类型的变量设为false
								flag_illegal=true;
						}
						break;

						default://都不匹配, 那就设为空, 也即不显示任何内容(这个脚本模板默认显示错误信息)
						unit.mode_display=DisplayUnit.DisplayMode.None;
						break;
					}
				}

				if(flag_illegal)
					unit.mode_display=DisplayUnit.DisplayMode.None;

				//自动设置LCD的显示模式
				if(unit.flag_graphic)
				{
					pair.Key.ContentType=ContentType.SCRIPT;//设置为 脚本模式
					pair.Key.Script="";//脚本设为 None
					pair.Key.ScriptBackgroundColor=Color.Black;//黑色背景色
				}
				else
					pair.Key.ContentType=ContentType.TEXT_AND_IMAGE;//设置为 文字与图片模式

				//添加到列表
				list__display_units.Add(unit);
			}
		}


		//初始化脚本配置
		void init_script_config()
		{
			//脚本配置实例
			config_script=new CustomDataConfig(Me);

			config_set__script=new CustomDataConfigSet("SCRIPT CONFIGURATION");

			//添加配置项

			//下一行是为配信息中添加说明信息, 这样用户可以更加明确配置的内容
			config_set__script.add_line("MODE OF THE SCRIPT");
			config_set__script.add_config_item(nameof(mode_script),() => mode_script,x => { mode_script=(ScriptMode)x; });
			
			//TODO: 在这里添加你的脚本配置项

			//将配置集合添加到配置对象中
			config_script.add_config_set(config_set__script);

			//解析一次CD数据
			config_script.parse_custom_data();
		}

		#endregion

		#region 类型定义

		//脚本运行模式
		public enum ScriptMode
		{
			Regular,//常规模式

			//TODO: 在这里添加你的脚本运行模式, 可以只有一个模式, 也可以没有任何模式
		}

		//类 显示单元(当成结构体用)
		class DisplayUnit
		{
			//枚举 显示模式
			public enum DisplayMode
			{
				General,//一般信息显示

				//TODO: 在这里添加其它的显示模式, 这些模式决定了脚本如何输出到LCD上

				None,//不显示内容
			}

			//LCD显示器
			//public IMyTextPanel lcd;
			//显示器
			public IMyTextSurface displayer;

			public DisplayMode mode_display = DisplayMode.General;

			//索引 开始 (列表容器中的索引)
			public int index_begin = -1;
			//索引 末尾 (列表容器中的索引)
			public int index_end = -1;
			//标记 是否图形化显示
			public bool flag_graphic = false;

			//构造函数
			public DisplayUnit(IMyTextSurface _display,int _index_begin = -1,int _index_end = -1,bool _flag_graphic = false)
			{
				displayer=_display;
				flag_graphic=_flag_graphic;
				index_begin=_index_begin;
				index_end=_index_end;
			}
		}

		#endregion

		#region 配置通用

		/***************************************************************************************************
		* 类 自定义数据配置
		* 自定义数据配置(下简称CD配置)使用目标方块的自定义数据来进行脚本配置
		* 支持动态配置, 标题等, 功能强大
		* 使用方法:
		* 1. 创建 CustomDataConfigSet 对象, 调用 add_config_item() 函数添加配置项
		* 2. 创建 CustomDataConfig 对象, 调用 add_config_set() 函数添加集合对象
		* 3. 添加配置项集合之后, 调用 CustomDataConfig 的 parse_custom_data() 函数
		* 4. 调用之后会自动更新绑定的变量, 之后可以调用 write_to_block_custom_data() 函数进行写入
		* 5. write_to_block_custom_data() 是必须调用的, 这样保证当脚本第一次运行而CD中没有数据时,
		*    脚本会自动在CD中生成数据
		* 6. 你也可以使用 init_config() 便携函数, 内部执行了以上两个函数
		* 
		* 如果需要使用动态配置, 先执行一次 parse_custom_data(), 这时绑定的变量已经更新, 
		* 之后再决定是否添加下一个 CustomDataConfigSet 对象, 注意, 添加配置对象之后需要 执行一次
		* 
		***************************************************************************************************/

		//管理对象
		public class CustomDataConfig
		{
			//分割线 标题
			public static string separator_title = "##########";
			//分割线 副标题
			public static string separator_subtitle = "-----";
			//映射表 配置项集合
			Dictionary<string,CustomDataConfigSet> dict__config_sets = new Dictionary<string,CustomDataConfigSet>();
			//映射表 字符串内容
			Dictionary<string,List<string>> dict__str_contents = new Dictionary<string,List<string>>();
			//字符串构建器 数据
			public StringBuilder string_builder__data { get; private set; } = new StringBuilder();
			//字符串构建器 错误信息
			public StringBuilder string_builder__error_info { get; private set; } = new StringBuilder();

			//终端方块 CD配置的目标方块
			public IMyTerminalBlock block_target { get; private set; }

			//标记 配置中发现错误(存在错误时不会覆盖写入)
			public bool flag__config_error { get; private set; } = false;

			public CustomDataConfig(IMyTerminalBlock block_target)
			{
				this.block_target=block_target;
			}

			//初始化配置
			public void init_config()
			{
				parse_custom_data();//解析自定义数据
				if(!flag__config_error)//检查CD配置是否存在错误
					write_to_block_custom_data();//写入自定义数据
			}

			//添加配置集
			public bool add_config_set(CustomDataConfigSet set)
			{
				if(dict__config_sets.ContainsKey(set.title__config_set))
					return false;
				dict__config_sets.Add(set.title__config_set,set);
				return true;
			}

			//解析CD(拆分)
			public void parse_custom_data()
			{
				//以换行符拆分
				string[] array_lines = block_target.CustomData.Split('\n');
				string pattern = $"{separator_title} (.+) {separator_title}";//正则表达式
				var regex = new System.Text.RegularExpressions.Regex(pattern);
				string title_crt = "";
				foreach(var line in array_lines)
				{
					var match = regex.Match(line);//正则匹配
					if(line.Length==0) continue;
					else if(match.Success)
						dict__str_contents[title_crt=match.Groups[1].ToString()]=new List<string>();
					else if(dict__str_contents.ContainsKey(title_crt))
						dict__str_contents[title_crt].Add(line);
					else
					{
						string_builder__error_info.Append($"<error> illegal CD config data: \n{line}\n");
						flag__config_error=true; break;
					}
				}
				foreach(var pair in dict__str_contents)
				{
					if(dict__config_sets.ContainsKey(pair.Key))
						if(!dict__config_sets[pair.Key].parse_string_data(pair.Value))
						{
							string_builder__error_info.Append($"<error> illegal config item in config set: [{pair.Key}]\n{dict__config_sets[pair.Key].string_builder__error_info}\n");
							flag__config_error=true; break;
						}
				}
			}

			//写入方块CD
			public void write_to_block_custom_data()
			{
				foreach(var item in dict__config_sets)
				{
					item.Value.generate_string_data();
					string_builder__data.Append(item.Value.string_builder__data);
				}
				block_target.CustomData=string_builder__data.ToString();
			}
		}
		//CD配置集合
		public class CustomDataConfigSet
		{
			//类 配置项指针
			public class ConfigItemReference
			{
				//读委托
				public Func<object> get { get; private set; }

				//写委托
				public Action<object> set { get; private set; }

				//构造函数 传递委托(委托类似于函数指针, 用于像指针那样读写变量)
				public ConfigItemReference(Func<object> _getter,Action<object> _setter)
				{
					get=_getter; set=_setter;
				}
			}

			//配置集标题
			public string title__config_set { get; private set; }

			//字典 配置项字典
			Dictionary<string,ConfigItemReference> dict__config_items = new Dictionary<string,ConfigItemReference>();

			//字符串构建器 数据
			public StringBuilder string_builder__data { get; private set; } = new StringBuilder();
			//字符串构建器 错误信息
			public StringBuilder string_builder__error_info { get; private set; } = new StringBuilder();

			//标记 配置中发现错误(存在错误时不会覆盖写入)
			public bool flag__config_error { get; private set; } = false;

			//构造函数
			public CustomDataConfigSet(string title_config = "SCRIPT CONFIGURATION")
			{
				this.title__config_set=title_config;
			}

			//添加配置项
			public bool add_config_item(string name_config_item,Func<object> getter,Action<object> setter)
			{
				if(dict__config_items.ContainsKey(name_config_item))
					return false;
				dict__config_items.Add(name_config_item,new ConfigItemReference(getter,setter));
				return true;
			}

			//添加分割线
			public bool add_line(string str_title)
			{
				//检查是否已经包含此配置项目
				if(dict__config_items.ContainsKey(str_title))
					return false;
				//添加到字典
				dict__config_items.Add(str_title,null);
				return true;
			}

			//更新配置项的值
			//public bool update_config_item_value(string name__config_item, object value__config_item, bool flag_rewrite = true)
			//{
			//	if (!dict__config_items.ContainsKey(name__config_item))
			//		return false;
			//	dict__config_items[name__config_item].set(value__config_item);
			//	//重新写入
			//	if (flag_rewrite)
			//		write_to_block_custom_data();
			//	return true;
			//}

			//解析字符串
			public bool parse_string_data(List<string> content)
			{
				int count = 0;

				foreach(var line in content)
				{
					++count;
					if(line.Length==0)
						continue;//跳过空行
					if(line.StartsWith(CustomDataConfig.separator_subtitle))
						continue;
					//以等号拆分
					string[] pair = line.Split('=');
					//检查
					if(pair.Length!=2)
					{
						string_builder__error_info.Append($"<error> \"{line}\"(at line {count}) is not a legal config item");
						flag__config_error=true;
						continue;//跳过配置错误的行
					}

					//去除多余空格
					string name__config_item = pair[0].Trim();
					string str_value__config_item = pair[1].Trim();

					ConfigItemReference reference;
					//尝试获取
					if(!dict__config_items.TryGetValue(name__config_item,out reference))
						continue;//不包含值, 跳过

					//包含值, 需要解析并更新
					var value = reference.get();//获取值
					if(parse_string(str_value__config_item,ref value))
						//成功解析字符串, 更新数值
						dict__config_items[name__config_item].set(value);
					else
					{
						//解析失败
						string_builder__error_info.Append($"<error> \"{str_value__config_item}\"(at line {count}) is not a legal config value");
						flag__config_error=true;
					}
				}
				return !flag__config_error;
			}

			//生成字符串数据
			public void generate_string_data()
			{
				int count = 0;

				string_builder__data.Clear();
				string_builder__data.Append($"{CustomDataConfig.separator_title} {title__config_set} {CustomDataConfig.separator_title}\n");
				foreach(var pair in dict__config_items)
				{
					if(pair.Value!=null)
						string_builder__data.Append($"{pair.Key} = {pair.Value.get()}\n");
					else
					{
						if(count!=0) string_builder__data.Append("\n");
						string_builder__data.Append($"{CustomDataConfig.separator_subtitle} {pair.Key} {CustomDataConfig.separator_subtitle}\n");
					}
					++count;
				}
				string_builder__data.Append("\n\n");
			}

			//解析字符串值
			private bool parse_string(string str,ref object v)
			{
				if(v is bool)
				{
					bool value_parsed;
					if(bool.TryParse(str,out value_parsed))
					{
						v=value_parsed;
						return true;
					}
				}
				else if(v is float)
				{
					float value_parsed;
					if(float.TryParse(str,out value_parsed))
					{
						v=value_parsed;
						return true;
					}
				}
				else if(v is double)
				{
					double value_parsed;
					if(double.TryParse(str,out value_parsed))
					{
						v=value_parsed;
						return true;
					}
				}
				else if(v is int)
				{
					int value_parsed;
					if(int.TryParse(str,out value_parsed))
					{
						v=value_parsed;
						return true;
					}
				}
				else if(v is Vector3D)
				{
					Vector3D value_parsed;
					if(Vector3D.TryParse(str,out value_parsed))
					{
						v=value_parsed;
						return true;
					}
				}
				else if(v is string)
				{
					v=str;
					return true;
				}
				else if(v is ScriptMode)
				{
					ScriptMode value_parsed;
					if(ScriptMode.TryParse(str,out value_parsed))
					{
						v=value_parsed;
						return true;
					}
				}

				//TODO: 如果需要让配置模块支持你自定义的类型(或其它类型), 在这里进行扩展

				return false;
			}
		}

		#endregion

#if DEVELOP
	}
}
#endif
