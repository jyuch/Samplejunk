package utils.auth

import com.mohiva.play.silhouette.api.Authorization
import com.mohiva.play.silhouette.impl.authenticators.CookieAuthenticator
import models.User
import play.api.mvc.Request

import scala.concurrent.Future

/**
  * Only allows those managers that have at least a role of the selected.
  * Master role is always allowed.
  * Ex: WithRole("high", "sales") => only managers with roles "high" OR "sales" (or "master") are allowed.
  */
case class WithRole(anyOf: String*) extends Authorization[User, CookieAuthenticator] {
  def isAuthorized[A](manager: User, authenticator: CookieAuthenticator)(implicit r: Request[A]) = Future.successful {
    WithRole.isAuthorized(manager, anyOf: _*)
  }
}
object WithRole {
  def isAuthorized(manager: User, anyOf: String*): Boolean =
    anyOf.intersect(manager.roles).size > 0 || manager.roles.contains("master")
}

/**
  * Only allows those managers that have every of the selected roles.
  * Master role is always allowed.
  * Ex: Restrict("high", "sales") => only managers with roles "high" AND "sales" (or "master") are allowed.
  */
case class WithRoles(allOf: String*) extends Authorization[User, CookieAuthenticator] {
  def isAuthorized[A](manager: User, authenticator: CookieAuthenticator)(implicit r: Request[A]) = Future.successful {
    WithRoles.isAuthorized(manager, allOf: _*)
  }
}
object WithRoles {
  def isAuthorized(manager: User, allOf: String*): Boolean =
    allOf.intersect(manager.roles).size == allOf.size || manager.roles.contains("master")
}