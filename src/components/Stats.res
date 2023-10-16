open Constants
open Styles

let stats = Array.mapWithIndex(stats, (stat, _index) => {
  let stats_bg = `bg-black-gradient p-5 rounded-xl`
  <div
    key={stat["id"]} className={`${stats_bg} ml-10 flex-1 flex justify-start items-center flex-row m-3`}>
    <h4
      className="font-poppins font-semibold xs:text-[40px] text-[30px] xs:leading-[50px] leading-[43px] text-white">
      {stat["value"]->React.string}
    </h4>
    <p
      className="font-poppins font-normal xs:text-[20px] text-[15px] xs:leading-[26px] leading-[21px] text-gradient ml-3 uppercase">
      {stat["title"]->React.string}
    </p>
  </div>
})

@react.component
let make = () => {
  <section className={`${styles["flexCenter"]} flex-row flex-wrap sm:mb-20 mb-6`}>
    {stats->React.array}
  </section>
}
