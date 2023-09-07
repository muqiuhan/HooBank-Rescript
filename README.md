<div align="center">

<img src="./.github/logo.png" height="150">

# FsmDB

*WiscKey based Key-Value Store implementation in F#*

![](https://img.shields.io/badge/.NET%20Core%208.0.100~preview.7-8A2BE2)

![](https://img.shields.io/badge/.NET%20Core%20Native%20AOT-8A2BE2)
![](https://github.com/muqiuhan/FsmDB/actions/workflows/build.yml/badge.svg)

__WIP: This project is developed in [The X-Files Research Institute](https://github.com/X-FRI/FsmDB)__

</div>

## Introduction
WiscKey is a persistent LSM-tree-based key-value store with a performance-oriented data layout that separates keys from values to minimize I/O amplification. The design of WiscKey is highly SSD optimized, leveraging both the sequential and random performance characteristics of the device. 

- original paper: [https://www.usenix.org/system/files/conference/fast16/fast16-papers-lu.pdf](https://www.usenix.org/system/files/conference/fast16/fast16-papers-lu.pdf)

## Build and Test

- Build: `dotnet build`
- Publish: `dotnet publish -r [platform]`
- Test: `dotnet test`

## Dependencies
- The implementation of SkipList uses [DataStructures/SkipList.cs](https://github.com/sphinxy/DataStructures/blob/master/DataStructures/SkipList.cs)

## Reference
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

## License
The MIT License (MIT)

Copyright (c) 2022 Muqiu Han

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