<div align="center">

<img src="./.github/logo.png" height="150">

# Suka

*WiscKey based Key-Value database implementation in Scala*

__WIP: This project is developed in [The X-Files Research Institute](https://github.com/X-FRI/Suka)__


![Scala 3.3.0](https://img.shields.io/badge/Scala3.3.0-%23DC322F)
![SBT 1.9.6](https://img.shields.io/badge/SBT1.9.6-%23380D09)

![Build and Test](https://github.com/x-fri/Suka/actions/workflows/BuildAndTest.yaml/badge.svg)

</div>

## Components

- Cli
- Query Parser
- Query Engine
- Storage Engine

```
     +-----+
     | cli |
     +-----+
        |
        v
 +--------------+          +--------+
 | Query Engine | <------> | Parser |
 +--------------+          +--------+
        |
        v
+----------------+
| Storage Engine |
+----------------+
```

## Parser
Using the pre-compiled MySQL ANTLR parser from [Apache ShardingSphere](https://shardingsphere.apache.org/) to parse SQL, tokenize the inputs, create the domain object, and send it to the Query Engine. 

Parser will supports:
- SELECT
- CREATE
- INSERT
- DELETE
- UPDATE
- DROP

## Query Engine
The query engine abstracts all the downstream interactions. Query Engine internally calls Parser to parse the SQL statements and convert it to Domain objects. The Domain Objects are then used by the Query Planner to create a query execution plan based on some rules or heuristics. The Query Plan is executed by the Execution Engine. The Execution engine talks to Storge Engine by scanning disk files and reading the table records. These records are then send back to Query Engine via Iterator Pattern. The Query Engine applies filters based on Selection and Projection Operators defined in the Query Plan and finally sends it back to the CLI for printing the table output.

## Storage Engine
WiscKey is a persistent LSM-tree-based key-value store with a performance-oriented data layout that separates keys from values to minimize I/O amplification. The design of WiscKey is highly SSD optimized, leveraging both the sequential and random performance characteristics of the device. 

# Reference
- [Apache ShardingSphere](https://shardingsphere.apache.org/)
- [WiscKey original paper: Separating Keys from Values in SSD-Conscious Storage](https://www.usenix.org/system/files/conference/fast16/fast16-papers-lu.pdf)
- [The Log-Structured Merge-Tree (LSM-Tree) original paper](https://www.cs.umb.edu/~poneil/lsmtree.pdf) 
- [Introductory tutorial to designing and building a LSM-Tree based Key-Value Store like RocksDB](https://adambcomer.com/blog/simple-database/motivation-design/)
- [Wikipedia: Key–value database](https://en.wikipedia.org/wiki/Key%E2%80%93value_database)
- [Wikipedia: Log-structured merge-tree](https://en.wikipedia.org/wiki/Log-structured_merge-tree)
- [Wikipedia: Skip List](https://en.wikipedia.org/wiki/Skip_list)
- [RocksDB Wiki: MemTable](https://github.com/facebook/rocksdb/wiki/MemTable)
- [RocksDB Wiki: BlockBasedTable Format](https://github.com/facebook/rocksdb/wiki/Rocksdb-BlockBasedTable-Format)
- [RocksDB: Write Ahead Log](https://github.com/EighteenZi/rocksdb_wiki/blob/master/Write-Ahead-Log.md)
- [RocksDB source code: A Persistent Key-Value Store for Flash and RAM Storage](https://github.com/facebook/rocksdb)
- [WiscKey C implementation by @adambcomer](https://github.com/adambcomer/WiscKey)
- [Build a tiny database in Java](https://medium.com/javarevisited/build-a-tiny-database-in-java-ca6d3f06e115)

## License
The MIT License (MIT)

Copyright (c) 2023 Muqiu Han

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
