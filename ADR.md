# Architecture Decision Records

## Decisions

1. Offset vs Cursor
<br>
    > Key finding in this project is the cursor pagination is generally faster than offset pagination as the application displaying and retrieving a deeper rows. The performance quite similar at the beginning but the gap become exponentially higher towards the end of the pages.
    > However, both have their own pros and cons, and which choice is "better" very much depending on your use case. Cursor pagination shine for infinite scrolling (commonly use in social media feed display) while offset pagination will be useful in much more traditional layout where the system displays data page by page.
    > In an application that displays data row by row but may requires a consistent rendering time across all page, they may opt for a hybrid implementation. They can query first page using offset but for later query, they may switch to using cursor. However, please be noted that this may introduce an unnecessary complexity to the code.
    > Another way to "mitigate the slowness" of the offset pagination is to implement something called "pre-fetching strategy". When the user query page 1, the system can quitely query page 2 and store it locally (either by local storage or cache) so that it can be returned fastly when the user requesting it. However, this, just like the previous suggestion, also comes with complexity.

2. When to materialize the query
<br>
    > There are two side of opinions regarding this. The purists believe that anything related to the infrastructure concerns - in this case is IQueryable materlaization - should stays in the infrastructure layer. The pragmatists on the other hands, query materialization should happen as late as possible - even up to the presentation layer as materializing it too early may comes with a cost like memory overhead.
    > As for myself, I am going for the second one. "I am okay" with exposing the infrastructure concerns for the performance and memory. While we may eventually have to materialize it right before the http response, we basically push the overhead to the later flow. This stand is also applies for implementation of `SaveChangeAsync()`.
    > Extra notes: Specification pattern might be useful as the middle ground but may introduce complexity, which means more work to do! 

3. Bulk update implementation
<br>
    > In this project, I am just relying on the simple `savechangeasync()` call instead of other available options such as `UpdateRange()`, `ExecuteUpdate()`, and `BulkUpdateAsync()` library. Here are why I am choosing and not choosing them.
    > `SaveChangeAsync()` is simple and straightforward. The change in an entity may be varied. Entity A may change only property `x` while entity B may change property `y` and `z`. Looading the requested entities to memory is a trade-off that is worth taking for this use case. This way, since I am also using `Unit of Work` pattern and is abled to call `SaveChangeAsync()` directly, I don't have to create a specific method in repository class to handle this.
    > `UpdateRange()` is just `SaveChangeAsync()` with extra steps as I have to create a specific method in repository class to handle this. Plus, it is bad for performance as it will update all columns every time even though the user might not initiate it.
    > `ExecuteUpdate` is great for use case where the value of the updated property is the same across all entities. Totally a big "NO" for this use case.
    > `BulkUpdateAsync()` may be the most suitable for this use case but it is an external libraries which i try not to use in this `proof-of-concept` project. Certainly is useful for enterprise-level applications.


## Rule of Thumbs

- On selection of methods to use:
  > If two implementations have the same impact, always pick the one with clearer intent. If two implementations have the same intent, always pick the one with better impact.