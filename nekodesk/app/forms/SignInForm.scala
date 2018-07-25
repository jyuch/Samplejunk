package forms

import play.api.data.Form
import play.api.data.Forms._

/**
 * The form which handles the submission of the credentials.
 */
object SignInForm {

  /**
   * A play framework form.
   */
  val form = Form(
    mapping(
      "loginId" -> nonEmptyText,
      "password" -> nonEmptyText,
      "rememberMe" -> boolean
    )(Data.apply)(Data.unapply)
  )

  case class Data(
    loginId: String,
    password: String,
    rememberMe: Boolean)
}
