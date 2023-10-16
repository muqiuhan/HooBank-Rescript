open Constants
open Styles

@react.component
let make = () => {
  <section className={`${styles["flexCenter"]} my-4`}>
    <div className={`${styles["flexCenter"]} flex-wrap w-full`}>
      {Array.map(clients, client => {
        <div
          key={client["id"]}
          className={`flex-1 ${styles["flexCenter"]} sm:min-w-[192px] min-w-[120px] m-5`}>
          <img
            src={client["logo"]} alt="client_logo" className="sm:w-[192px] w-[100px] object-contain"
          />
        </div>
      })->React.array}
    </div>
  </section>
}
