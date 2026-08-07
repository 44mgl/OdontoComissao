import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { PublicLayout } from '../layouts/PublicLayout'
import { HomePage } from '../pages/Home/HomePage'
import { EventsPage } from '../pages/Events/EventsPage'
import { CommissionPage } from '../pages/Commission/CommissionPage'
import { ShopPage } from '../pages/Shop/ShopPage'
import { ReservationPage } from '../pages/Reservation/ReservationPage'
import { PlaceholderPage } from '../pages/Placeholder/PlaceholderPage'

export function AppRoutes() {
  return (
    <BrowserRouter> 
      <Routes>
        <Route element={<PublicLayout />}>
          <Route index element={<HomePage />} />
          <Route path="eventos" element={<EventsPage />} />
          <Route path="comissao" element={<CommissionPage />} />
          <Route path="shop" element={<ShopPage />} />
          <Route path="reserva" element={<ReservationPage />} />
          <Route path="vip/login" element={<PlaceholderPage />} />
          <Route path="admin/login" element={<PlaceholderPage />} />
          <Route path="*" element={<PlaceholderPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
