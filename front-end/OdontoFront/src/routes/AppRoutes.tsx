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
            element={<ProtectedRoute allowedRole="VIP" loginPath="/vip/login" />}
          >
            <Route path="vip" element={<PlaceholderPage />} />
          </Route>

          <Route
            element={(
              <ProtectedRoute
                allowedRole="Administrador"
                loginPath="/admin/login"
              />
            )}
          >
            <Route path="admin" element={<PlaceholderPage />} />
          </Route>

          <Route path="*" element={<PlaceholderPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
