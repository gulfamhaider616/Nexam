using Nexam.Models.Entities;

namespace Nexam.Models.ViewModels;

/// <summary>What a paper card shows. One card design is used for every exam body (PPSC, NTS, FPSC, CSS).</summary>
/// <param name="Paper">The paper.</param>
/// <param name="Url">Where the card links to.</param>
/// <param name="Badge">The small label at the top (the year, or the exam body).</param>
/// <param name="Footer">The line at the bottom left (for PPSC: the post).</param>
public sealed record PaperCardModel(Paper Paper, string Url, string Badge, string Footer);

/// <summary>A titled grid of paper cards.</summary>
public sealed record PaperListModel(string Title, IEnumerable<PaperCardModel> Cards);
