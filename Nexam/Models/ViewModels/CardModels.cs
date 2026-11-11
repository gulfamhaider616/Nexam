using Nexam.Models.Entities;

namespace Nexam.Models.ViewModels;

/// <summary>What a paper card shows. One card design is used for every exam body (PPSC, NTS, FPSC, CSS).</summary>
/// <param name="Paper">The paper.</param>
/// <param name="Url">Where the card links to.</param>
/// <param name="Badge">The small label at the top (the year, or the exam body). Also what the year chips filter on.</param>
/// <param name="Footer">The line at the bottom left (for PPSC: the post).</param>
public sealed record PaperCardModel(Paper Paper, string Url, string Badge, string Footer);

/// <summary>A titled grid of paper cards, with an optional live search box and year chips.</summary>
public sealed record PaperListModel(string Title, IEnumerable<PaperCardModel> Cards, string Subtitle = null, bool Filterable = true);

/// <summary>One question in a list (subject page, search results, a paper's questions).</summary>
/// <param name="Question">The question (Options already split).</param>
/// <param name="Number">The number shown on the card.</param>
/// <param name="DetailUrl">Link for the statement (the discussion page), or null for plain text.</param>
/// <param name="ShowSubject">Show the subject badge.</param>
public sealed record QuestionCardModel(Question Question, int Number, string DetailUrl = null, bool ShowSubject = true);
