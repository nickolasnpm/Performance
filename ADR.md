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

    > In this project, i am choosing to use external party nuget package `BulkUpdateAsync()` instead of other available options such as default `SaveChangeasync()`, `UpdateRange()`, and `ExecuteUpdate()`.
    
    > `BulkUpdateAsync()` generates a single SQL command for bulk operation and can cater for the use case where each updated entities may have different property values. For example, entity A may change only property `x` while entity B may change property `y` and `z`. This mean all the entities need to be loaded into the memory in order for the update operation to take place and for property value to be manipulated. This action in a way will resulted in extra memory overhead. Other than that, a specific method in repository class is needed to handle this operation - which in some way is supporting a clear separation of concerns since all database operation is done in the infrastructure layer.
    
    > `SaveChangeAsync()` is simple and straightforward. Similar to `BulkUpdateAsync()`, it support non-uniform value manipulation and will requires the updated entities to be loaded into memory in order for the update operation to take place. It will also helpful in the case where the system is also implementing the `Unit of Work` pattern and `SaveChangeAsync()` can be called directly from the service. In this case, no specific update method in repository class is needed to handle this. However, since the database operation is executed directly in the service class, it can be considered as anti-pattern for `Clean Architecture` with strict separation of concerns rule.
    
    > `UpdateRange()` is just `SaveChangeAsync()` with extra steps as I have to create a specific method in repository class to handle this. Plus, it is bad for performance as it will update all columns every time even though the user might not initiate it.
    
    > `ExecuteUpdate()` generates a single SQL command for bulk operation and executes the command directly in the database (`SaveChangeAsync()` generate unique SQL command for each entity). It aslo does not requires neither entity loading nor change tracking. Hence, due to this nature, `ExecuteUpdate()` resulted in blazingly fast one round trip and zero memory allocation. However, the better use case for `ExecuteUpdate()` is where the value of the updated property is similar and uniform across all entities. For that reason, it is totally a big "NO" for this use case.

- Bulk delete implementation

    > In this project, I am using `ExecuteDeleteAsync()` method which will executes directly n the database - bypassing `SaveChangeAsync()` - and is not tracked back by the EF change tracker resulted in fast one round database trip and less memory allocation. It is also working very well in supporting `OnDelete` behaviors such as Cascade or Restrict.
    > `RemoveAsync()` is good but that requires the entire entity to be loaded into the memory and having multiple delete process may resulted in multiple database trip. Besides, as you can see from my `DeleteUsers()` method in `UserService()` class as well, it is only the Ids that need to be loaded into memory - making `RemoveAsync()` the least suitable choice.

- Unit of work implementation

    > There are two side of opinions regarding this. The first opinion put the entire trust in EF core to handle everything. Their use cases may include application that do mostly 1 database transaction per http request, or multiple database transactions per http request but all the transactions are handled inside 1 big class. The first opinion priotize the convenience and simplicity and sacrificing the separation of concerns between infrastructure and application layer.

    > The second opinion requires the manual intervention to ensure single atomic transaction in the application that comes with the implementation of `Unit of work` pattern. The requirement often comes from the nature of the application itself where it requires multiple database transactions per http request and each of the transaction have to go through it owns complex business logic. This opinion prioritize the modularity between classes in the application layer and strict separation of concerns between layers and tolerating code complexity.

    > In this project, I am going for the `Unit of work` pattern as there are multiple request that requires multiple transactions within it. Apart from that, I also want to make sure that the `SaveChangeAsync()` is only called after all transaction is deemed completed and to free every service from the responsibility of calling `SaveChangeAsync()` by their own every time.

- ID Encyption vs GUID/UUID

    > The usage of ID Encryption is spefically to protect the anonymity of `long` primary key ID but may be overkill the application of this scale.  Using `long` as primary key ID and as clustered index is beneficial in many aspects: (1) smallest storage size among the options, (2) natively support an index scan during read operation, and (3) helpful to avoid index fragmentation write operation. Hence, the usage of ID encryption/decryption is to support high performance operation in the backend while hiding the insight on database design and business scale from the outside world. However, the addition of ID encryption/decryption process to every http call will definitely add a little bit of complexity and operational overhead which I believe is tolerable and negligible if compared with the alternatives.

    > The alternative to this is `GUID/UUID`. If it is used as primary key especially in distributed system, it can also be used as reference ID, and thus avoiding the need to maintain multiple extra columns (If the primay key is `long` Id, additional reference ID may be needed to provide uniqueness). However, it may introduce a bit of memory overhead since `GUID/UUID` storage size in MSSQL Server is 16 bytes (`long` has 8 bytes storage size). At the same time it may introduce latency to http call in both read and write operations as it does not natively support the index scan operations and will cause index fragmentation. If it is used as reference ID (in the case `long` is used as primary key ID), developer may need to introduce extra columns which will resulted in several drawbacks such as (1) extra maintenance overhead, (2) additional complexity as developers need to decide upfront on whether that particular table will be referenced by other tables or not, and (3) potential performance impact as each table might have to have extra index for read operations.

- Result Pattern

    > Result pattern is useful to make explicit return from application layer to API layer. Instead of calling the exception direcly which may cause extra overhead mainly (1) search operation for matching exception catch and (2) extra memory for both exception, debugging info, and stack trace metadata, Result pattern support similar handling pattern for both success and error operations.
    
    > However, this may introduce some development complexity as the system have multiple way to handle error (`concentional exception handling` is still helpful to capture unknown and unhandled error) and confusion to the new developer that might not have proper understanding of this pattern. The code for result pattern can be very verbose.

## Rule of Thumbs

- On selection of methods to use:
  > If two implementations have the same impact, always pick the one with clearer intent. If two implementations have the same intent, always pick the one with better impact.
