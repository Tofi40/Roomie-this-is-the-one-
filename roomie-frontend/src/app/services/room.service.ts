import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Room } from '../models/room.model';
import { RoomPhoto } from '../models/room-photo.model';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private readonly roomUrl = `${environment.apiUrl}/api/Room`;
  private readonly roomPhotoUrl = `${environment.apiUrl}/api/RoomPhoto`;

  constructor(private http: HttpClient) {}

  getAllRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(this.roomUrl);
  }

  getRoomById(id: number): Observable<Room> {
    return this.http.get<Room>(`${this.roomUrl}/${id}`);
  }

  createRoom(room: Partial<Room>): Observable<Room> {
    return this.http.post<Room>(this.roomUrl, room);
  }

  updateRoom(room: Room): Observable<void> {
    return this.http.put<void>(this.roomUrl, room);
  }

  deleteRoom(id: number): Observable<void> {
    return this.http.delete<void>(`${this.roomUrl}/${id}`);
  }

  getPhotosByRoomId(roomId: number): Observable<RoomPhoto[]> {
    return this.http.get<RoomPhoto[]>(`${this.roomPhotoUrl}/room/${roomId}`);
  }
}
