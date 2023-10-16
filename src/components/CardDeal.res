open Styles

@module("../assets/card.png") external card_image: string = "default"

@react.component
let make = () => {
  <section className={`${layout["section"]}`}>
    <div className={`${layout["sectionInfo"]}`}>
      <h2 className={`${styles["heading2"]}`}>
        {"Find a better card deal"->React.string}
        <br className="sm:block hidden" />
        {"in few easy steps."->React.string}
      </h2>
      <p className={`${styles["paragraph"]} max-w-[470px] mt-5`}>
        {"Arcu tortor, purus in mattis at sed integer faucibus. Aliquet quis
        aliquet eget mauris tortor.ç Aliquet ultrices ac, ametau."->React.string}
      </p>
      <Button styles="mt-10" />
    </div>
    <div className={`${layout["sectionImg"]}`}>
      <img src={card_image} alt="billing" className="w-[100%] h-[100%]" />
    </div>
  </section>
}
