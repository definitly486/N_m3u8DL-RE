using N_m3u8DL_RE.Common.Util;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Collections.Concurrent;

namespace N_m3u8DL_RE.Column;

internal class RecordingDurationColumn : ProgressColumn
{
    protected override bool NoWrap => true;
    private ConcurrentDictionary<int, int> _recodingDurDic;
    private ConcurrentDictionary<int, int>? _refreshedDurDic;
    public Style GreyStyle { get; set; } = new Style(foreground: Color.Grey);
    public Style MyStyle { get; set; } = new Style(foreground: Color.DarkGreen);
    public RecordingDurationColumn(ConcurrentDictionary<int, int> recodingDurDic)
    {
        _recodingDurDic = recodingDurDic;
    }
    public RecordingDurationColumn(ConcurrentDictionary<int, int> recodingDurDic, ConcurrentDictionary<int, int> refreshedDurDic)
    {
        _recodingDurDic = recodingDurDic;
        _refreshedDurDic = refreshedDurDic;
    }
  public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
{
    // Безопасно получаем основную длительность (в секундах)
    if (!_recodingDurDic.TryGetValue(task.Id, out var dur))
    {
        dur = 0; // если ключа нет — 0 секунд
    }

    if (_refreshedDurDic == null)
    {
        return new Text($"{GlobalUtil.FormatTime(dur)}", MyStyle)
            .LeftJustified();
    }

    // Безопасно получаем обновлённую длительность
    if (!_refreshedDurDic.TryGetValue(task.Id, out var refDur))
    {
        refDur = 0; // если ключа нет — 0 секунд
    }

    return new Text(
        $"{GlobalUtil.FormatTime(dur)}/{GlobalUtil.FormatTime(refDur)}",
        GreyStyle
    );
}
}
