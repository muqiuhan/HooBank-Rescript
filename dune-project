(lang dune 3.7)

(name lsmdb)

(generate_opam_files true)

(source
 (github muqiuhan/lsmdb))

(authors "Muqiu Han")

(maintainers "Muqiu Han")

(license LICENSE)

(documentation https://github.com/muqiuhan/lsmdb)

(package
 (name lsmdb)
 (synopsis "LSM-Tree based Key-Value Store")
 (description "LSM-Tree based Key-Value Store")
 (depends ocaml dune)
 (tags
  (database sql)))

; See the complete stanza docs at https://dune.readthedocs.io/en/stable/dune-files.html#dune-project
