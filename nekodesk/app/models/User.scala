package models

import java.util.UUID

import com.mohiva.play.silhouette.api.{ Identity, LoginInfo }

/**
 * The user object.
 *
 * @param userID The unique ID of the user.
 * @param loginInfo The linked login info.
 * @param fullName Maybe the full name of the authenticated user.
 */
case class User(
  userID: UUID,
  loginInfo: LoginInfo,
  fullName: String,
  roles: Seq[String]
  ) extends Identity {

  /**
   * Tries to construct a name.
   *
   * @return Maybe a name.
   */
  def name = fullName
}
