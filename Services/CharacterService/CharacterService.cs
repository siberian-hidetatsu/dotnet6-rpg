using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        // sql express
        // private static List<Character> characters = new List<Character>
        // {
        //     new Character(),
        //     new Character { Id = 1, Name = "Sam"}
        // };

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //characters.Add(_mapper.Map<Character>(newCharacter));
            Character character = _mapper.Map<Character>(newCharacter);
            //character.Id = characters.Max(c => c.Id) + 1;
            //characters.Add(character);
            ////serviceResponse.Data = characters;
            //serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            // sql express
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            //return characters;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            //return characters;
            //return new ServiceResponse<List<GetCharacterDto>> {Data = characters};

            // return new ServiceResponse<List<GetCharacterDto>>
            // {
            //     Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
            // };
            // sql express のための処理
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
                .Where(c => c.User.Id == userId)    // userID に関連したキャラクターのみ抽出するため
                .ToListAsync();
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
           var serviceResponse = new ServiceResponse<GetCharacterDto>();
           //var character = characters.FirstOrDefault(c => c.Id == id);
           //serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
           // sql express
           var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
           serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
           //return characters.FirstOrDefault(c => c.Id == id);
           return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

            try
            {
                //Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
                // sql express
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                // _mapper.Map(updatedCharacter, character);
                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                // sql express
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch ( Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                //Character character = characters.First(c => c.Id == id);
                //characters.Remove(character);
                //response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                // sql express
                Character character = await _context.Characters.FirstAsync(c => c.Id == id);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                response.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch ( Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }
    }
}