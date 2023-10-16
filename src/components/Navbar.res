@module("../assets/logo.svg") external logo: string = "default"
@module("../assets/close.svg") external close_icon: string = "default"
@module("../assets/menu.svg") external menu_icon: string = "default"

let menu = mobile => {
  let end_menu_item_margin = index => {
    if mobile {
      Array.length(Constants.navLinks) - 1 == index ? "mr-0" : "mb-4"
    } else if Array.length(Constants.navLinks) - 1 == index {
      "mr-0"
    } else {
      "mr-10"
    }
  }

  Array.mapWithIndex(Constants.navLinks, (nav, index) => {
    <li
      key={`${nav["id"]}`}
      className={`${end_menu_item_margin(
          index,
        )} font-poppins font-normal cursor-pointer text-[16px] text-white`}>
      <a href={`${nav["id"]}`}> {nav["title"]->React.string} </a>
    </li>
  })->React.array
}

@react.component
let make = () => {
  let (toggle, setToggle) = React.useState(() => false)
  let mobile_menu_icon = toggle ? close_icon : menu_icon
  let mobile_menu_toggle = toggle ? "flex" : "hidden"

  <nav className="w-full flex py-6 justfiy-between items-center navbar">
    <img src=logo alt="hoobank" className="w-[124px] h-[32px]" />
    <ul className="list-none sm:flex hidden justify-end items-center flex-1"> {menu(false)} </ul>
    <div className="sm:hidden flex flex-1 justify-end items-center">
      <img
        src={`${mobile_menu_icon}`}
        className="w-[28px] h-[28px] object-contain"
        alt="menu"
        onClick={event => setToggle(toggle => !toggle)}
      />
      <div
        className={`${mobile_menu_toggle} p-6 bg-black-gradient absolute top-20 right-0 mx-4 my-2 min-w-[140px] rounded-xl sidebar`}>
        <ul className="list-none flex justify-end items-center flex-1 flex-col"> {menu(true)} </ul>
      </div>
    </div>
  </nav>
}
