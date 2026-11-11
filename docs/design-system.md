# Nexam design system

Single stylesheet `Nexam/wwwroot/css/nexam.css`, single script `Nexam/wwwroot/js/nexam.js`. No Bootstrap, no jQuery, no icon fonts.
Mobile-first; light and dark themes (tokens swap under `[data-theme="dark"]`, saved in `localStorage`, `?theme=light|dark` previews one).

Reference pages to copy patterns from: `Views/Home/Index.cshtml`, `Subjects.cshtml`, `GetAllQuestionsBySubjectID.cshtml`, `SearchResult.cshtml`.

## Layouts
| Layout | Use |
|---|---|
| `Views/Shared/_Layout.cshtml` | public pages. Provides header, mobile drawer, footer. Sections: `Head` (extra `<link>`), `Scripts` (extra `<script>`). |
| `Views/Shared/_AdminLayout.cshtml` | admin pages. Sidebar nav + top bar; content goes in `.admin-main`. Same sections. |

Every page sets `ViewBag.Title`, `ViewBag.Description`, `ViewBag.KeyWords` (public) and `Layout = "~/Views/Shared/_Layout.cshtml"`.

## Page skeleton (public)
```html
<div class="container">
  <div class="page-head">
    <nav class="breadcrumbs" aria-label="Breadcrumb"><a href="/">Home</a><span>/</span><span aria-current="page">Title</span></nav>
    <h1>Title</h1>
    <p>One-line description.</p>
  </div>
  <div class="section" style="padding-top:20px"> ... </div>
</div>
```
Two-column page: `<div class="section layout-sidebar">` with main column + `<aside class="sticky stack">` of `.panel`s.
Narrow centred content (forms, text): `<div class="container container-narrow">`.

## Layout helpers
`.container`, `.container-narrow`, `.section`, `.section-head` (title left, action right), `.grid-cards` (auto-fill cards, min 280px), `.grid-2`, `.stack` (vertical gap, `style="--gap:12px"`), `.cluster` (wrapping row with gap), `.muted`, `.text-center`, `.mt-2/.mt-3/.mt-4`, `.mb-0`, `.hidden`, `.sr-only`.

## Components
- Buttons: `.btn` + `.btn-primary | .btn-secondary | .btn-ghost | .btn-danger`, sizes `.btn-sm | .btn-lg`, `.btn-block`.
- Forms: `.form-card` (white card) > `.form-grid` (`.cols-2` for two columns) > `.field` (label + control + `.help`). Controls: `.input`, `.select`, `.textarea`. Checkbox: `<label class="check"><input type="checkbox"> text</label>`. Buttons row: `.form-actions`.
- Cards/panels: `.card` (+ `.card-body`), `.panel` (padded box with `<h3>`), `.badge` (+ `-muted | -success | -danger | -amber`), `.chip` (pill link/button; `aria-pressed="true"` = selected), `.alert` (+ `-danger | -success`), `.empty` (empty state with icon + text).
- Hero / CTA: `.hero`, `.cta-band`, `.feature-grid` > `.feature`.
- Tables (admin, lists): `<div class="table-wrap"><table class="table">...</table></div>`; row actions in `<div class="actions">` of `.btn-sm`.
- Stats / admin tiles: `.stat-grid` > `.stat` (`<strong>` number + `<span>` label); `.admin-tiles` > `a.admin-tile` (`.tile` icon + `<h3>` + `<p>`).
- Threads (forum): `a.thread` (`.meta` row with badges).
- Prose (about/terms): `<div class="prose">` with h2/p/ul.
- Auth pages: `<div class="container auth-wrap"><div class="auth-card form-card">...</div></div>`.

## Icons
Inline SVG sprite (`_Icons.cshtml`), used as `<svg class="icon" aria-hidden="true"><use href="#i-NAME"/></svg>`.
Available: search menu close sun moon arrow-right book check user mail quiz message home plus edit trash eye chevron-right file-text calendar layers trophy clock download logout sliders shield inbox globe filter external info.
Add a new `<symbol id="i-NAME" viewBox="0 0 24 24">` (stroke paths, no fill) to `_Icons.cshtml` if needed.

## Shared partials (always reuse these; do not copy their markup)
| Partial | Model | Use |
|---|---|---|
| `_PaperList` | `PaperListModel(title, cards, subtitle?, filterable?)` | whole page of paper cards with live filter + year chips |
| `_PaperCard` | `PaperCardModel(paper, url, badge, footer)` | one paper card |
| `_SubjectCard` | `Subject` | one subject card |
| `_QuestionCard` | `QuestionCardModel(question, number, detailUrl?, showSubject?)` | one question with options and highlighted answer (`Options` must be populated: services do this for lists) |

```cshtml
@await Html.PartialAsync("_QuestionCard", new QuestionCardModel(q, number++, "/PPSC-Past-Paper-Question-Discussion/" + q.QuestionID, ShowSubject: false))
```

## Question lists
Wrap the list in `data-filter-scope` to get a live filter, and use `.q-list` for the cards:
```html
<div data-filter-scope>
  <div class="filters">
    <div class="search-field"><svg class="icon" aria-hidden="true"><use href="#i-search"/></svg>
      <input class="input" type="search" placeholder="Filter these questions" data-filter-input></div>
    <div class="filter-meta"><span data-filter-count></span>
      <label class="switch"><input type="checkbox" data-answers-toggle><span>Show answers</span></label></div>
  </div>
  <div class="q-list"> ...question cards... </div>
  <div class="empty hidden" data-filter-empty>...</div>
</div>
```
Data attributes understood by `nexam.js`: `data-confirm="Are you sure?"` (on a link or button: asks before following/submitting), `data-filter-scope`, `data-filter-input`, `data-filter-chip="value"` (`""` = all), `data-filter-item` (+ optional `data-filter-group`, `data-filter-text`), `data-filter-count`, `data-filter-empty`, `data-answers-toggle`, `data-theme-toggle`, `data-menu-toggle`, `data-search-input`.

## Dialog
```js
NexamDialog.show({ title: 'Thank you!', html: '<p>We will review your question.</p>',
                   actions: [{ label: 'Go to forum', href: '/Nexam-Forum', primary: true }] });
```

## Rules for page markup
- Semantic HTML first; one `<h1>` per page; every input has a `<label>` (or `aria-label`).
- Use `dir="auto"` on user/question text (Urdu content).
- No Bootstrap grid (`row`, `col-*`), no `wow`/`fa`/`flaticon`, no inline `<script>` that needs jQuery. Small inline `style` for one-off spacing only.
- Keep form `action`/`method`, input `name` attributes, hidden fields, route URLs and element ids that scripts use exactly as they were.
