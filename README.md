# Myitian.NbtSerDes
一个NBT标签序列化/反序列化器

兼容以下任一运行时

.NET Framework 4.5及以上

.NET Core 2.0及以上

.NET 5.0及以上

# 示例

## 序列化/反序列化一个结构方块文件格式 

（如果字段/属性名与目标NBT标签名一致，可不使用NbtProperty属性）

先定义如下结构方块文件类：

```csharp
public class StructureNBTRoot
{
    [NbtProperty("DataVersion")]
    public int DataVersion { get; set; }
    [NbtProperty("author")]
    public string Author { get; set; }
    [NbtProperty("size"), NbtType(NbtType.TAG_List)]
    public int[] Size { get; set; }
    [NbtProperty("palette")]
    public StructureNBTPaletteItem[] Palette { get; set; }
    [NbtProperty("palettes")]
    public StructureNBTPaletteItem[][] Palettes { get; set; }
    [NbtProperty("blocks")]
    public StructureNBTBlock[] Blocks { get; set; }
    [NbtProperty("entities")]
    public StructureNBTEntity[] Entities { get; set; }
}
public class StructureNBTPaletteItem
{
    [NbtProperty("Name")]
    public string Name { get; set; }
    [NbtProperty("Properties")]
    public Dictionary<string, string> Properties { get; set; }
}
public class StructureNBTBlock
{
    [NbtProperty("state")]
    public int State { get; set; }
    [NbtProperty("pos"), NbtType(NbtType.TAG_List)]
    public int[] Pos { get; set; }
    [NbtProperty("nbt")]
    public dynamic Nbt { get; set; }
}
public class StructureNBTEntity
{
    [NbtProperty("pos"), NbtType(NbtType.TAG_List)]
    public double[] Pos { get; set; }
    [NbtProperty("bolckPos"), NbtType(NbtType.TAG_List)]
    public int[] BolckPos { get; set; }
    [NbtProperty("nbt"), NbtRequired]
    public dynamic Nbt { get; set; }
}
```

再使用如下代码（假设nbt文件在程序启动路径）：

```csharp
// 反序列化
byte[] nbtbytes = File.ReadAllBytes(@"with_mast.nbt");
StructureNBTRoot nbt = NbtConverter.DeserializeObject<StructureNBTRoot>(nbtbytes);

// 序列化
byte[] nbtbytes2 = NbtConverter.SerializeObject(nbt);
```