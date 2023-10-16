// Generated by ReScript, PLEASE EDIT WITH CARE

import * as Button from "./Button.bs.js";
import * as Styles from "../Styles.bs.js";
import * as Constants from "../Constants.bs.js";
import * as JsxRuntime from "react/jsx-runtime";

function Business$FeatureCard(props) {
  var feature = props.feature;
  var mb = props.index !== (Constants.features.length - 1 | 0) ? "mb-6" : "mb-9";
  return JsxRuntime.jsxs("div", {
              children: [
                JsxRuntime.jsx("div", {
                      children: JsxRuntime.jsx("img", {
                            className: "w-[50%] h-[50%] object-contain",
                            alt: "icon",
                            src: feature.icon
                          }),
                      className: "" + Styles.styles.flexCenter + " w-[64px] h-[64px] rounded-full bg-dimBlue"
                    }),
                JsxRuntime.jsxs("div", {
                      children: [
                        JsxRuntime.jsx("h4", {
                              children: feature.title,
                              className: "font-poppins font-semibold text-white text-[18px] leading-[23px] mb-1"
                            }),
                        JsxRuntime.jsx("p", {
                              children: feature.content,
                              className: "font-poppins font-normal text-dimWhite text-[18px] leading-[23px] mb-1"
                            })
                      ],
                      className: "flex-1 flex flex-col ml-3"
                    })
              ],
              className: "" + mb + " flex flex-row p-6 rounded-[20px] feature-card"
            });
}

var FeatureCard = {
  make: Business$FeatureCard
};

function Business(props) {
  return JsxRuntime.jsxs("section", {
              children: [
                JsxRuntime.jsxs("div", {
                      children: [
                        JsxRuntime.jsxs("h2", {
                              children: [
                                "You do the business, ",
                                JsxRuntime.jsx("br", {
                                      className: "sm:block hidden"
                                    }),
                                "we'll handle the money."
                              ],
                              className: "" + Styles.styles.heading2 + ""
                            }),
                        JsxRuntime.jsx("p", {
                              children: "With the right credit card, you can improve your financial life by building credit, earning rewards and saving money. But with hundreds of credit cards on the market.",
                              className: "" + Styles.styles.paragraph + " max-w-[470px] mt-5"
                            }),
                        JsxRuntime.jsx(Button.make, {
                              styles: "mt-10"
                            })
                      ],
                      className: "" + Styles.layout.sectionInfo + ""
                    }),
                JsxRuntime.jsx("div", {
                      children: Constants.features.map(function (feature, index) {
                            return JsxRuntime.jsx(Business$FeatureCard, {
                                        feature: feature,
                                        index: index
                                      }, feature.id);
                          }),
                      className: "" + Styles.layout.sectionImg + " flex-col"
                    })
              ],
              className: "" + Styles.layout.section + ""
            });
}

var make = Business;

export {
  FeatureCard ,
  make ,
}
/* Button Not a pure module */
