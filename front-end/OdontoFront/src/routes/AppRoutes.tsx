import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { PublicLayout } from '../layouts/PublicLayout'
import { HomePage } from '../pages/Home/HomePage'
import { EventsPage } from '../pages/Events/EventsPage'
import { CommissionPage } from '../pages/Commission/CommissionPage'
import { ShopPage } from '../pages/Shop/ShopPage'
import { ReservationPage } from '../pages/Reservation/ReservationPage'
import { ReservationLookupPage } from '../pages/ReservationLookup/ReservationLookupPage'
import { PlaceholderPage } from '../pages/Placeholder/PlaceholderPage'
import { AdminLoginPage } from '../pages/AdminLogin/AdminLoginPage'
import { VipLoginPage } from '../pages/VipLogin/VipLoginPage'
import { ProtectedRoute } from './ProtectedRoute'
import { AuthorizationListener } from './AuthorizationListener'
import { AccessDeniedPage } from '../pages/AccessDenied/AccessDeniedPage'
import { VipLayout } from '../layouts/VipLayout'
import { VipDashboardPage } from '../pages/Vip/VipDashboardPage'
import { VipProductsPage } from '../pages/Vip/VipProductsPage'
import { VipProfilePage } from '../pages/Vip/VipProfilePage'
import { VipReservationsPage } from '../pages/Vip/VipReservationsPage'
import { AdminLayout } from '../layouts/AdminLayout'
import { AdminDashboardPage } from '../pages/Admin/AdminDashboardPage'
import { AdminResourcePage } from '../pages/Admin/AdminResourcePage'
import { AdminProductsPage } from '../pages/Admin/AdminProductsPage'
import { AdminReservationsPage } from '../pages/Admin/AdminReservationsPage'
import {
  administratorConfig,
  commissionConfig,
  eventConfig,
  publicationConfig,
  vipConfig,
} from '../pages/Admin/resourceConfig'

export function AppRoutes() {
  return (
    <BrowserRouter> 
      <AuthorizationListener />
      <Routes>
        <Route element={<PublicLayout />}>
          <Route index element={<HomePage />} />
          <Route path="eventos" element={<EventsPage />} />
          <Route path="comissao" element={<CommissionPage />} />
          <Route path="shop" element={<ShopPage />} />
          <Route path="reserva" element={<ReservationPage />} />
          <Route path="reserva/:codigo" element={<ReservationLookupPage />} />
          <Route path="consultar-reserva" element={<ReservationLookupPage />} />
          <Route path="vip/login" element={<VipLoginPage />} />
          <Route path="admin/login" element={<AdminLoginPage />} />
          <Route path="acesso-negado" element={<AccessDeniedPage />} />

          <Route
            element={(
              <ProtectedRoute
                allowedRoles={['VIP', 'Administrador']}
                loginPath="/vip/login"
              />
            )}
          >
            <Route path="vip" element={<VipLayout />}>
              <Route index element={<VipDashboardPage />} />
              <Route path="produtos" element={<VipProductsPage />} />
              <Route
                element={<ProtectedRoute allowedRoles={['VIP']} loginPath="/vip/login" />}
              >
                <Route path="reserva" element={<ReservationPage mode="vip" />} />
                <Route path="reservas" element={<VipReservationsPage />} />
                <Route path="perfil" element={<VipProfilePage />} />
              </Route>
            </Route>
          </Route>

          <Route
            element={(
              <ProtectedRoute
                allowedRoles={['Administrador']}
                loginPath="/admin/login"
              />
            )}
          >
            <Route path="admin" element={<AdminLayout />}>
              <Route index element={<AdminDashboardPage />} />
              <Route path="publicacoes" element={<AdminResourcePage config={publicationConfig} />} />
              <Route path="eventos" element={<AdminResourcePage config={eventConfig} />} />
              <Route path="comissao" element={<AdminResourcePage config={commissionConfig} />} />
              <Route path="produtos" element={<AdminProductsPage />} />
              <Route path="reservas" element={<AdminReservationsPage />} />
              <Route path="vips" element={<AdminResourcePage config={vipConfig} />} />
              <Route path="administradores" element={<AdminResourcePage config={administratorConfig} />} />
            </Route>
          </Route>

          <Route path="*" element={<PlaceholderPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
