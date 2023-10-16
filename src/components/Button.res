@react.component
let make = (~styles) => {
  <button
    className={`${styles} bg-blue-gradient py-4 px-6 font-poppins font-medium text-[18px] outline-none text-primary`}>
    {"Get Started"->React.string}
  </button>
}
