[![Build status](https://ci.appveyor.com/api/projects/status/7ay8uj64ucfnb7ab?svg=true)](https://ci.appveyor.com/project/jej666/skeleton) 
[![Build Status](https://travis-ci.org/jej666/Skeleton.svg?branch=master)](https://travis-ci.org/jej666/Skeleton)
[![Coverage Status](https://coveralls.io/repos/github/jej666/Skeleton/badge.svg?branch=master)](https://coveralls.io/github/jej666/Skeleton?branch=master)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# Skeleton
> Loosely-coupled solution to quickly build REST self-hosted web services.


Skeleton intends to help developping high performance self-hosted REST services, providing generic implementations of common software scenarii. 
Based on the onion architecture, Skeleton uses dependency injection to decouple infrastructure (Orm, Logging, Data, ...) from Business concerns (Domain entities, rules, ...) and web servicing. 
Concrete examples are provided in test solutions.

## Installation

Clone or Fork the solution and open it in Visual Studio 2015

## Usage example
1. Define a domain entity, specifying the database primary key
```csharp
public class Customer : EntityBase<Customer>  
{
    public Customer() : base(e => e.CustomerId)  { }
    public int CustomerId { get; set; }
    public int CustomerCategoryId { get; set; }
    public string Name { get; set; }
}
```

2. Query data with a simple ORM, without any configuration or attributes (Sync, Async and Cached modes available)
* Sync
```csharp
// Resolve dependency
var reader = Container.Resolve<IEntityReader<Customer>>();
// Simple Query
var results = reader.Where(c => c.Name.Equals(customer.Name) && c.CustomerId >= 1)
                    .OrderBy(c => c.CustomerId)
                    .Find();
                   
// Selected Columns
var results = reader.Where(c => c.CustomerId == customer.CustomerId)
                    .Select(c => c.CustomerId)
                    .Find();
                    
// Aggregations
var sum = reader.OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Sum(c => c.CustomerCategoryId);
               
// Joins
var results = reader.LeftJoin<CustomerCategory>((customer, category) =>
                        customer.CustomerCategoryId == category.CustomerCategoryId)
                    .Find();

// Plus all SQL keywords omitted for brevity 
```
* Async
```csharp
var reader = Container.Resolve<IAsyncEntityReader<Customer>>();
var results = await reader.Where(c => c.Name.Equals(customer.Name) && c.CustomerId >= 1)
                          .OrderBy(c => c.CustomerId)
                          .FindAsync();
```

* Cached
```csharp
var reader = Container.Resolve<ICachedEntityReader<Customer>>();
reader.CacheConfigurator = config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
var results = reader.Where(c => c.Name.Equals(customer.Name) && c.CustomerId >= 1)
                          .OrderBy(c => c.CustomerId)
                          .Find();
```

3. Persist data (Sync and Async modes)
* Sync
```csharp
var writer = Container.Resolve<IEntityWriter<Customer>>();
writer.Add(customer);
writer.Update(customer);
writer.Delete(customer);
```
* Async
```csharp
var writer = Container.Resolve<IAsyncEntityWriter<Customer>>();
writer.AddAsync(customer);
writer.UpdateAsync(customer);
writer.DeleteAsync(customer);
```

4. Define an entity DTO
```csharp
public class CustomerDto 
{
    public int CustomerId { get; set; }
    public int CustomerCategoryId { get; set; }
    public string Name { get; set; }
}
```

5. Expose a webapi controller
* Sync

```csharp
public class CustomersController : EntityCrudController<Customer, CustomerDto>  
{
    public CustomersController(
        ILogger logger,
        IEntityReader<Customer> reader,
        IEntityMapper<Customer, CustomerDto> mapper,
        IEntityWriter<Customer> writer)
        : base(logger, reader, mapper, writer)  
        { }
}
```

* Async

```csharp
public class AsyncCustomersController : AsyncEntityCrudController<Customer, CustomerDto> 
{
    public AsyncCustomersController(
        ILogger logger,
        IAsyncEntityReader<Customer> reader,
        IEntityMapper<Customer, CustomerDto> mapper,
        IAsyncEntityWriter<Customer> writer)
        : base(logger, reader, mapper, writer)
        { }
}
```

* Cached

```csharp
public class CachedCustomersController : CachedEntityReaderController<Customer, CustomerDto> 
{
    public CachedCustomersController(
        ILogger logger,
        ICachedEntityReader<Customer> reader,
        IEntityMapper<Customer, CustomerDto> mapper)
        : base(logger, reader, mapper)
        { }
}
```

6. Use a CRUD REST service (Sync and Async modes)

JSON format with GZip or deflate compression, with fault resiliency (exponential backoff retry)
```csharp
var client = new CrudHttpClient<CustomerDto>("http://localhost:8081/api/customers");
// Create
client.Create(customerDto);
// Read
client.FirstOrDefault(id: 1);
client.GetAll();
client.Page(pageSize: 20, pageNumber: 1);
// Update
client.Update(customerDto)
// Delete
client.Delete(customerDto);
```

7. The self-hosted Owin pipeline can be started very simply for testing purposes or used as a windows service

```csharp
public sealed class OwinServer : IDisposable 
{
    private IDisposable _server;
    
    public void Dispose()  
    {
        _server.Dispose();
    }
    
    public void Start(Uri baseUrl)  
    {
        // Database configuration
        Bootstrapper.UseDatabase(builder => builder.UsingConfigConnectionString("Default").Build());
        
        // Start HttpListener
        _server = Startup.StartServer(baseUrl);
    } 
}
```

8. WebApi is self documented using Swagger

![swagger](https://cloud.githubusercontent.com/assets/6336801/25340702/d7eb98ea-2906-11e7-8586-a0303f206a09.PNG)


## Development setup

All development dependencies are nuget-based. Unit tests can directly be executed in Visual Studio using UnitTesting tools

## Release History

* 1.0.0
    * Initial commit

## Meta

Jérôme Quiles – jejquiles@gmail.com

Distributed under the MIT license. See ``LICENSE`` for more information.
