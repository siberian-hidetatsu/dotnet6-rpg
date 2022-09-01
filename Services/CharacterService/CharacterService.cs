using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Sam"}
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //characters.Add(_mapper.Map<Character>(newCharacter));
            Character character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            //serviceResponse.Data = characters;
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            //return characters;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            //return characters;
            //return new ServiceResponse<List<GetCharacterDto>> {Data = characters};
            return new ServiceResponse<List<GetCharacterDto>>
            {
                Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
           var serviceResponse = new ServiceResponse<GetCharacterDto>();
           var character = characters.FirstOrDefault(c => c.Id == id);
           serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
           //return characters.FirstOrDefault(c => c.Id == id);
           return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

            try
            {
                Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

                // character.Name = updatedCharacter.Name;
                // character.HitPoints = updatedCharacter.HitPoints;
                // character.Strength = updatedCharacter.Strength;
                // character.Defense = updatedCharacter.Defense;
                // character.Intelligence = updatedCharacter.Intelligence;
                // character.Class = updatedCharacter.Class;
                _mapper.Map(updatedCharacter, character);

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
                Character character = characters.First(c => c.Id == id);
                characters.Remove(character);
                response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
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