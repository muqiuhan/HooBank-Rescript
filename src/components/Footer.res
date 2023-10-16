open Constants
open Styles

@module("../assets/logo.svg") external logo: string = "default"

@react.component
let make = () => {
  let link_mb = (footerlink, index) => {
    index != Array.length(footerlink["links"]) - 1 ? "mb-4" : "mb-0"
  }

  let social_mb = index => {
    index != Array.length(socialMedia) - 1 ? "mr-6" : "mr-0"
  }

  <section className={`${styles["flexCenter"]} ${styles["paddingY"]} flex-col`}>
    <div className={`${styles["flexStart"]} md:flex-row flex-col mb-8 w-full`}>
      <div className="flex-[1] flex flex-col justify-start mr-10">
        <img src={logo} alt="hoobank" className="w-[266px] h-[72.14px] object-contain" />
        <p className={`${styles["paragraph"]} mt-4 max-w-[312px]`}>
          {"A new way to make the payments easy, reliable and secure."->React.string}
        </p>
      </div>
      <div className="flex-[1.5] w-full flex flex-row justify-between flex-wrap md:mt-0 mt-10">
        {Array.map(footerLinks, footerlink => {
          <div key={footerlink["title"]} className={`flex flex-col ss:my-0 my-4 min-w-[150px]`}>
            <h4 className="font-poppins font-medium text-[18px] leading-[27px] text-white">
              {footerlink["title"]->React.string}
            </h4>
            <ul className="list-none mt-4">
              {Array.mapWithIndex(footerlink["links"], (link, index) => {
                <li
                  key={link["name"]}
                  className={`${link_mb(
                      footerlink,
                      index,
                    )} font-poppins font-normal text-[16px] leading-[24px] text-dimWhite hover:text-secondary cursor-pointer`}>
                  {link["name"]->React.string}
                </li>
              })->React.array}
            </ul>
          </div>
        })->React.array}
      </div>
    </div>
    <div
      className="w-full flex justify-between items-center md:flex-row flex-col pt-6 border-t-[1px] border-t-[#3F3E45]">
      <p className="font-poppins font-normal text-center text-[18px] leading-[27px] text-white">
        {"Copyright Ⓒ 2022 HooBank. All Rights Reserved."->React.string}
      </p>
      <div className="flex flex-row md:mt-0 mt-6">
        {Array.mapWithIndex(socialMedia, (social, index) => {
          <img
            key={social["id"]}
            src={social["icon"]}
            alt={social["id"]}
            className={`${social_mb(index)} w-[21px] h-[21px] object-contain cursor-pointer`}
            // onClick={() => window.open(social.link)}
          />
        })->React.array}
      </div>
    </div>
  </section>
}
