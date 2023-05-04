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
 (synopsis "Simple LSM-Tree database implementation")
 (description "Simple LSM-Tree database implementation")
 (depends ocaml dune)
 (tags
  (database "lsm-tree" sql)))

; See the complete stanza docs at https://dune.readthedocs.io/en/stable/dune-files.html#dune-project
