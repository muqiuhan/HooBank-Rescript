<div align="center">

<img src="./.github/logo.png" height="150">

FsmDB
> LSM-Tree based Key-Value Store implementation in F#

</div>

This is a learning project with well-commented and readability-first code design goals, 
After the project completes the proof-of-concept phase, it will be re-implemented using OCaml, and the OCaml version will focus on performance.

## Build and Run
- Build: `dotnet build`
- Run: `dotnet run --project FsmDB`
- Test: `dotnet test`

## Reference
- Introductory tutorial to designing and building a LSM-Tree based Key-Value Store like RocksDB : [https://adambcomer.com/blog/simple-database/motivation-design/](https://adambcomer.com/blog/simple-database/motivation-design/)
- RocksDB: A Persistent Key-Value Store for Flash and RAM Storage : [https://github.com/facebook/rocksdb](https://github.com/facebook/rocksdb)
- Key–value database wikipedia : [https://en.wikipedia.org/wiki/Key%E2%80%93value_database](https://en.wikipedia.org/wiki/Key%E2%80%93value_database)
- The Log-Structured Merge-Tree (LSM-Tree) original paper : [https://www.cs.umb.edu/~poneil/lsmtree.pdf](https://www.cs.umb.edu/~poneil/lsmtree.pdf)
- Log-structured merge-tree wikipedia : [https://en.wikipedia.org/wiki/Log-structured_merge-tree](https://en.wikipedia.org/wiki/Log-structured_merge-tree)
- MemTable RocksDB wiki : [https://github.com/facebook/rocksdb/wiki/MemTable](https://github.com/facebook/rocksdb/wiki/MemTable)
- Rocksdb BlockBasedTable Format RocksDB wiki : [https://github.com/facebook/rocksdb/wiki/Rocksdb-BlockBasedTable-Format](https://github.com/facebook/rocksdb/wiki/Rocksdb-BlockBasedTable-Format)
- Skip list wikipedia : [https://en.wikipedia.org/wiki/Skip_list](https://en.wikipedia.org/wiki/Skip_list)
- Write AHead Log RocksDB wiki : [https://github.com/EighteenZi/rocksdb_wiki/blob/master/Write-Ahead-Log.md](https://github.com/EighteenZi/rocksdb_wiki/blob/master/Write-Ahead-Log.md)
- Write Ahead Log File Format RocksDB Wiki : [https://github.com/facebook/rocksdb/wiki/Write-Ahead-Log-File-Format#record-format](https://github.com/facebook/rocksdb/wiki/Write-Ahead-Log-File-Format#record-format)

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