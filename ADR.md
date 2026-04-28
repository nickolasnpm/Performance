# Architecture Decision Records

## Decisions

- Offset vs Cursor

    > Key finding in this project is the cursor pagination is generally faster than offset pagination as the application displaying and retrieving a deeper rows. The performance quite similar at the beginning but the gap become exponentially higher towards the end of the pages.
    > However, both have their own pros and cons, and which choice is "better" very much depending on your use case. Cursor pagination shine for infinite scrolling (commonly use in social media feed display) while offset pagination will be useful in much more traditional layout where the system displays data page by page.
    > In an application that displays data row by row but may requires a consistent rendering time across all page, they may opt for a hybrid implementation. They can query first page using offset but for later query, they may switch to using cursor. However, please be noted that this may introduce an unnecessary complexity to the code.
    > Another way to "mitigate the slowness" of the offset pagination is to implement something called "pre-fetching strategy". When the user query page 1, the system can quitely query page 2 and store it locally (either by local storage or cache) so that it can be returned fastly when the user requesting it. However, this, just like the previous suggestion, also comes with complexity and potential back navigation inconsistency without proper handling.

- When to materialize the query

    > There are two side of opinions regarding this. The purists believe that anything related to the infrastructure concerns - in this case is IQueryable materlaization - should stays in the infrastructure layer. The pragmatists on the other hands, query materialization should happen as late as possible - even up to the presentation layer and break the rule of separaation of concerns, as materializing it too early may comes with a cost like memory overhead.
    > As for myself, I am going for the second one. "I am okay" with exposing the infrastructure concerns for the performance and memory. While we may eventually have to materialize it right before the http response, we basically push the overhead to the later flow. This stand is also applies for implementation of `SaveChangeAsync()`.
    > Extra notes: Specification pattern might be useful as the middle ground but may introduce complexity, which means more work to do!

- Bulk update implementation

    > In this project, I am just relying on the simple `savechangeasync()` call instead of other available options such as `UpdateRange()`, `ExecuteUpdate()`, and `BulkUpdateAsync()` library. Here are why I am choosing and not choosing them.
    > `SaveChangeAsync()` is simple and straightforward. The change in an entity may be varied. Entity A may change only property `x` while entity B may change property `y` and `z`. Looading the requested entities to memory is a trade-off that is worth taking for this use case. This way, since I am also using `Unit of Work` pattern and is abled to call `SaveChangeAsync()` directly, I don't have to create a specific method in repository class to handle this.
    > `UpdateRange()` is just `SaveChangeAsync()` with extra steps as I have to create a specific method in repository class to handle this. Plus, it is bad for performance as it will update all columns every time even though the user might not initiate it.
    > `ExecuteUpdate()` executes the command directly in the database without the need for change tracking resulted in blazingly fast one round trip and zero memory allocation. However, the better use case for `ExecuteUpdate` is where the value of the updated property is similar across all entities. For that reason, it is totally a big "NO" for this use case.
    > `BulkUpdateAsync()` may be the most suitable for this use case but it is an external libraries which i try not to use in this `proof-of-concept` project. Certainly is useful for enterprise-level applications.

- Bulk delete implementation

    > In this project, I am using `ExecuteDeleteAsync()` method which will executes directly n the database - bypassing `SaveChangeAsync()` - and is not tracked back by the EF change tracker resulted in fast one round database trip and less memory allocation. It is also working very well in supporting `OnDelete` behaviors such as Cascade or Restrict.
    > `RemoveAsync()` is good but that requires the entire entity to be loaded into the memory and having multiple delete process may resulted in multiple database trip. Besides, as you can see from my `DeleteUsers()` method in `UserService()` class as well, it is only the Ids that need to be loaded into memory - making `RemoveAsync()` the least suitable choice.

- Unit of work implementation

    > There are two side of opinions regarding this. The first opinion put the entire trust in EF core to handle everything. Their use cases may include application that do mostly 1 database transaction per http request, or multiple database transactions per http request but all the transactions are handled inside 1 big class. The first opinion priotize the convenience and simplicity and sacrificing the separation of concerns between infrastructure and application layer.
    > The second opinion requires the manual intervention to ensure single atomic transaction in the application that comes with the implementation of `Unit of work` pattern. The requirement often comes from the nature of the application itself where it requires multiple database transactions per http request and each of the transaction have to go through it owns complex business logic. This opinion prioritize the modularity between classes in the application layer and strict separation of concerns between layers and tolerating code complexity.
    > In this project, I am going for the `Unit of work` pattern as there are multiple request that requires multiple transactions within it. Apart from that, I also want to make sure that the `SaveChangeAsync()` is only called after all transaction is deemed completed.

## Rule of Thumbs

- On selection of methods to use:
  > If two implementations have the same impact, always pick the one with clearer intent. If two implementations have the same intent, always pick the one with better impact.